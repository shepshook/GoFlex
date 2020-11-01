using System;
using System.Linq.Expressions;
using GoFlex.Core.Entities;
using GoFlex.Web.Services.Abstractions;
using GoFlex.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GoFlex.Web.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [Route("")]
        public IActionResult List(int? category, string order, int page = 1)
        {
            Expression<Func<Event, bool>> visibilityFilter = x => x.DateTime >= DateTime.Now;
            Enum.TryParse(typeof(EventListOrder), order, true, out var orderValue);

            var filter = new EventListFilter
            {
                CategoryId = category,
                OnlyApproved = true,
                AdditionalFilters = new[] {visibilityFilter},
                Ordering = (EventListOrder?) orderValue
            };

            var model = _eventService.GetPage(page, filter);
            
            if (page < 1 || page > model.Page.Total)
                return NotFound();

            return View(model);
        }

        [Route("[controller]/[action]/{id:int}")]
        public IActionResult Details(int id)
        {
            var item = _eventService.GetSingleEntity(id);
            return View(item);
        }
    }
}
