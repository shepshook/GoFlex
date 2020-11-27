using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GoFlex.Core.Entities;
using GoFlex.Core.Repositories.Abstractions;
using GoFlex.Web.Services.Abstractions;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Utils;
using QRCoder;
using Serilog;
using Stripe;
using Stripe.Checkout;
using ContentType = System.Net.Mime.ContentType;

namespace GoFlex.Web.Areas.Api
{
    [Area("Api")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly string _webhookSecret;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly IMailService _mailService;

        public PaymentController(IUnitOfWork unitOfWork, IConfiguration configuration, ILogger logger, IMailService mailService)
        {
            _unitOfWork = unitOfWork;
            _webhookSecret = configuration["Stripe:WebhookSecret"];
            _logger = logger.ForContext<PaymentController>();
            _configuration = configuration;
            _mailService = mailService;
        }

        [Authorize]
        [HttpPost("[area]/[controller]/[action]/{id:int}")]
        public ActionResult Create(int id, string returnUrl = null)
        {
            var host = Request.Scheme + Uri.SchemeDelimiter + Request.Host;

            var order = _unitOfWork.OrderRepository.Get(id);
            var user = _unitOfWork.UserRepository.Get(Guid.Parse(User.FindFirst("userId").Value));

            if (order == null || user.Id != order.UserId)
                return NotFound();

            var options = new SessionCreateOptions
            {
                Metadata = new Dictionary<string, string>
                {
                    {"OrderId", id.ToString()}
                },
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = order.Items.Select(item => new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = decimal.ToInt64(item.EventPrice.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = $"{order.Event.Name}: {item.EventPrice.Name}"
                        }
                    },
                    Quantity = item.Quantity
                }).ToList(),
                Mode = "payment",
                SuccessUrl = host + Url.Action("Success", "Order"),
                CancelUrl = host + Url.Action("Cancel", "Order"),
                CustomerEmail = user.Email
            };

            var service = new SessionService();
            var session = service.Create(options);
            _logger.Here().Information("New order created: {@Items}", options.LineItems.Select(x => new
            {
                Name = x.PriceData.ProductData.Name,
                Price = x.PriceData.UnitAmount,
                Qty = x.Quantity
            }));

            return new JsonResult(new {id = session.Id});
        }

        [HttpPost("api/[action]")]
        public async Task<IActionResult> Webhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _webhookSecret);

                _logger.Here().Information("Webhook activated for {@Event}", stripeEvent);

                Session session;

                switch (stripeEvent.Type)
                {
                    case Events.CheckoutSessionCompleted:
                        session = ExpandAsSession(stripeEvent);
                        if (session.PaymentStatus == "paid")
                            CompleteOrder(session);
                        break;

                    case Events.CheckoutSessionAsyncPaymentSucceeded:
                        session = ExpandAsSession(stripeEvent);
                        CompleteOrder(session);
                        break;

                    case Events.CheckoutSessionAsyncPaymentFailed:
                        session = ExpandAsSession(stripeEvent);
                        NotifyCustomer(session);
                        break;

                    default:
                        _logger.Here().Warning("Webhook event type {Type} is not supported: {@Event}", stripeEvent.Type, stripeEvent);
                        return BadRequest();
                }
            }
            catch (StripeException e)
            {
                _logger.Here().Warning("Webhook caused an {@Exception}", e);
                return BadRequest();
            }

            return Ok();
        }

        private Session ExpandAsSession(Stripe.Event stripeEvent)
        {
            var session = stripeEvent.Data.Object as Session;
            var service = new SessionService();
            
            var options = new SessionGetOptions();
            options.AddExpand("customer");

            return service.Get(session.Id, options);
        }

        private void CompleteOrder(Session session)
        {
            var id = int.Parse(session.Metadata["OrderId"]);
            var order = _unitOfWork.OrderRepository.Get(id);
            var emailReceiver = order.User.Email;

            _logger.Here().Information("Payment for order {Id} received from {Email}", order.Id, emailReceiver);

            foreach (var item in order.Items)
            {
                foreach (var _ in Enumerable.Range(0, item.Quantity))
                {
                    var secret = new OrderItemSecret
                    {
                        OrderItemId = item.Id,
                        Id = Guid.NewGuid(),
                        IsUsed = false
                    };
                    _unitOfWork.OrderItemSecretRepository.Insert(secret);
                }
            }
            _unitOfWork.Commit();

            var path = Request.Scheme + "://" + Request.Host.ToUriComponent();
            order = _unitOfWork.OrderRepository.Get(id);

            _mailService.SendOrder(order, path, Url);
        }

        private void NotifyCustomer(Session session)
        {
            //todo: notify customer about failed payment by email

            var id = int.Parse(session.Metadata["OrderId"]);
            var order = _unitOfWork.OrderRepository.Get(id);
            var email = session.Customer.Email;

            _logger.Here().Warning("Payment for order {@Order} failed for {Email}", order, email);
        }
    }
}
