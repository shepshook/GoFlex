using System;
using System.Linq.Expressions;
using GoFlex.Core.Entities;
using GoFlex.Web.Services.Abstractions;
using GoFlex.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GoFlex.Web.Controllers
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
        public IActionResult Events()
        {
            Expression<Func<Event, bool>> waitingFilter = x => x.IsApproved == null;
            var filter = new EventListFilter
            {
                AdditionalFilters = new [] {waitingFilter},
                Ordering = EventListOrder.CreateDate
            };
            var list = _eventService.GetList(filter);
            return View(list);
        }

        [HttpPost("[controller]/[action]")]
        public IActionResult Vote(int id, bool vote)
        {
            var result = _eventService.AcceptEvent(id, vote);
            if (!result)
                return NotFound();

            return RedirectToAction("Events");
        }
    }
}
