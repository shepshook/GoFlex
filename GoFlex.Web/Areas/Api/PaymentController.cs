using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GoFlex.Core.Repositories.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;

namespace GoFlex.Web.Areas.Api
{
    [Area("Api")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        //todo: move the key to configs
        private const string WebhookSecret = "whsec_hEosIrhMeJ4SQPwjYRqJwcmniWqUo0NX";
        private readonly IUnitOfWork _unitOfWork;

        public PaymentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("[area]/[controller]/[action]")]
        public ActionResult Create(int id, string returnUrl = null)
        {
            var host = Request.Scheme + Uri.SchemeDelimiter + Request.Host;

            var order = _unitOfWork.OrderRepository.Get(id);

            //todo: verify that authenticated user's id == order.UserId
            if (order == null)
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
                CancelUrl = host + Url.Action("Cancel", "Order")
            };

            var service = new SessionService();
            var session = service.Create(options);
            return new JsonResult(new {id = session.Id});
        }

        [HttpPost("[area]/[controller]/[action]")]
        public async Task<IActionResult> Complete()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], WebhookSecret);
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
                }
            }
            catch (StripeException e)
            {
                //todo: log an attempt of payment faking
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
            var email = session.Customer.Email;
        }

        private void NotifyCustomer(Session session)
        {
            //todo: notify customer about failed payment by email
        }
    }
}
