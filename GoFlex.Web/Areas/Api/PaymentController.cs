using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GoFlex.Core.Repositories.Abstractions;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MimeKit;
using QRCoder;
using Serilog;
using Stripe;
using Stripe.Checkout;

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

        public PaymentController(IUnitOfWork unitOfWork, IConfiguration configuration, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _webhookSecret = configuration["Stripe:WebhookSecret"];
            _logger = logger.ForContext<PaymentController>();
            _configuration = configuration;
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

        [HttpPost("webhook")]
        public async Task<IActionResult> Complete()
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
            //todo: compose an email message and send it to the customer
            //todo: an idea to generate a !unique! QR code that can be then verified by our api
            var id = int.Parse(session.Metadata["OrderId"]);
            var order = _unitOfWork.OrderRepository.Get(id);
            var emailReceiver = session.Customer.Email;
            _logger.Here().Information("Payment for order {Id} received from {Email}", order.Id, emailReceiver);

            var payload = new PayloadGenerator.Url(Request.Scheme + "://" + Request.Host.ToUriComponent() + Url.Action("List", "Event"));
            var asciiQr = new AsciiQRCode(new QRCodeGenerator().CreateQrCode(payload));

            var message = new MimeMessage
            {
                Subject = "Your order from GoFlex",
                Body = new TextPart(MimeKit.Text.TextFormat.Text)
                {
                    Text = asciiQr.GetGraphic(20)
                }
            };

            message.From.Add(new MailboxAddress("Self", _configuration["MailKit:Email"]));
            message.To.Add(new MailboxAddress("Self", emailReceiver));

            using var client = new SmtpClient();
            client.Connect(_configuration["MailKit:SmtpServer"], int.Parse(_configuration["MailKit:Port"]), true);
            client.Authenticate(_configuration["MailKit:Email"], _configuration["MailKit:Password"]);
            client.Send(message);
            client.Disconnect(true);

            _logger.Here().Information("Mail sent to {Email}", emailReceiver);
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
