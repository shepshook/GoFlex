using System;
using System.Collections.Generic;
using System.Linq;
using GoFlex.Core.Entities;
using GoFlex.Core.Repositories.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace GoFlex.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //todo: probably make a rest api version of this action with popup and move it to PaymentController
        [HttpPost("[controller]/[action]")]
        public IActionResult Confirm(int[] id, int?[] count)
        {
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

            //todo: provide ID of an authenticated user
            order.UserId = new Guid("3424F6C3-7327-41F6-988C-BB5E582661E7");

            _unitOfWork.OrderRepository.Insert(order);
            _unitOfWork.Commit();

            // Reload the order to populate nav props
            order = _unitOfWork.OrderRepository.Get(order.Id);

            return View(order);
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
