using System;
using System.Linq.Expressions;
using GoFlex.Core.Entities;
using GoFlex.Core.Repositories.Abstractions;
using GoFlex.Web.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace GoFlex.Web.Controllers
{
    public class EventController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public EventController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Route("")]
        public IActionResult List(int? category, string order, int page = 1)
        {
            Expression<Func<Event, bool>> visibilityFilter = x => x.DateTime >= DateTime.Now;
            Expression<Func<Event, bool>> categoryFilter = null;
            if (category.HasValue)
                categoryFilter = x => x.EventCategoryId == category;

            Expression<Func<Event, DateTime>> orderKeySelector;
            bool descending;

            Enum.TryParse(typeof(EventListOrder), order, true, out var orderValue);
            switch (orderValue)
            {
                case EventListOrder.CreateDate:
                    orderKeySelector = x => x.CreateTime;
                    descending = true;
                    break;
                case EventListOrder.Date:
                default:
                    orderKeySelector = x => x.DateTime;
                    descending = false;
                    break;
            }

            const int itemsPerPage = 12;

            //todo: add repository props class for filters and ordering parameters, provide them to all repos
            var events = _unitOfWork.EventRepository.GetPage(itemsPerPage, page, out var totalPages, orderKeySelector, descending, categoryFilter, visibilityFilter);

            if (page < 1 || page > totalPages)
                return NotFound();

            var pageViewModel = new PageViewModel(page, totalPages);
            if (category != null)
                pageViewModel.Parameters.Add("category", category.ToString());

            var model = new EventListViewModel
            {
                Events = events,
                Page = pageViewModel,
                EventCategories = _unitOfWork.EventCategoryRepository.All()
            };

            return View(model);
        }

        [Route("[controller]/[action]/{id:int}")]
        public IActionResult Details(int id)
        {
            var item = _unitOfWork.EventRepository.Get(id);
            return View(item);
        }

        //todo: authenticate as organizer
        [HttpGet("[controller]/[action]")]
        public IActionResult Add()
        {

        }
    }
}
