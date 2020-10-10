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

        [Route("[controller]/[action]")]
        public IActionResult Payment(int[] id, int?[] count)
        {
            var order = new Order
            {
                Items = new List<OrderItem>(Enumerable.Zip(id, count, (a, b) => new OrderItem {EventPriceId = a, Quantity = b ?? 0}).Where(x => x.Quantity != 0))
            };
            return View(order);
        }
    }
}
