using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GoFlex.Core.Entities;
using GoFlex.Services.Abstractions;
using GoFlex.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GoFlex.Controllers
{
    public class EventController : Controller
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [Route("")]
        public async Task<IActionResult> List(int? category, string order, int page = 1)
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

            var model = await _eventService.GetPage(page, filter);
            
            if (page < 1 || page > model.Page.Total && model.Page.Total != 0)
                return NotFound();

            return View(model);
        }

        [Route("[controller]/[action]/{id:int}")]
        public async Task<IActionResult> Details(int id)
        {
            var item = await _eventService.GetSingleEntity(id);
            return View(item);
        }
    }
}
