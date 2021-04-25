using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GoFlex.Core.Entities;
using GoFlex.Services.Abstractions;
using GoFlex.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoFlex.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IEventService _eventService;
        //private readonly ILocationService _locationService;

        public AdminController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [Route("[controller]/[action]")]
        public async Task<IActionResult> Events()
        {
            Expression<Func<Event, bool>> waitingFilter = x => x.IsApproved == null;
            var filter = new EventListFilter
            {
                AdditionalFilters = new [] {waitingFilter},
                Ordering = EventListOrder.CreateDate
            };
            var list = await _eventService.GetList(filter);
            return View(list);
        }

        [HttpPost("[controller]/[action]")]
        public async Task<IActionResult> Vote(int id, bool vote)
        {
            var result = await _eventService.AcceptEvent(id, vote);
            if (!result)
                return NotFound();

            return RedirectToAction("Events");
        }
    }
}
