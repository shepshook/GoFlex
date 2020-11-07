using System;
using System.Collections.Generic;
using System.Linq;
using GoFlex.Core.Entities;
using GoFlex.Core.Repositories.Abstractions;
using GoFlex.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace GoFlex.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly string _stripePublicKey;

        public OrderController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _stripePublicKey = configuration["Stripe:PublicKey"];
        }

        //todo: probably make a rest api version of this action with popup and move it to PaymentController
        [Authorize]
        [HttpPost("[controller]/[action]")]
        public IActionResult Confirm(int[] id, int?[] count)
        {
            //todo: move business logic to a service
            var order = new Order
            {
                Items = new List<OrderItem>(Enumerable
                    .Zip(id, count, (a, b) => new OrderItem {EventPriceId = a, Quantity = b ?? 0})
                    .Where(x => x.Quantity != 0)),
                Timestamp = DateTime.Now
            };

            if (!order.Items.Any())
                return BadRequest();

            order.Event = _unitOfWork.EventPriceRepository.Get(order.Items.First().EventPriceId).Event;

            order.UserId = Guid.Parse(User.FindFirst("userId").Value);

            _unitOfWork.OrderRepository.Insert(order);
            _unitOfWork.Commit();

            // Reload the order to populate nav props
            order = _unitOfWork.OrderRepository.Get(order.Id);

            var model = new OrderViewModel
            {
                Order = order,
                StripePublicKey = _stripePublicKey
            };

            return View(model);
        }

        [Route("[controller]/[action]")]
        public IActionResult Success()
        {
            return View();
        }

        [Route("[controller]/[action]")]
        public IActionResult Cancel()
        {
            return View();
        }
}
}
