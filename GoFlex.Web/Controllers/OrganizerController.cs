using System;
using GoFlex.ViewModels;
using GoFlex.Web.Services.Abstractions;
using GoFlex.Web.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace GoFlex.Web.Controllers
{
    [Authorize(Roles = "Admin,Organizer")]
    public class OrganizerController : Controller
    {
        private readonly IEventService _eventService;
        private readonly ILogger _logger;

        public OrganizerController(IEventService eventService, ILogger logger)
        {
            _eventService = eventService;
            _logger = logger.ForContext<OrganizerController>();
        }

        [Route("[controller]/[action]")]
        public IActionResult Events(int? category, bool? onlyActive, bool? onlyApproved, string order, int page = 1)
        {
            Enum.TryParse(typeof(EventListOrder), order, true, out var orderValue);

            var filter = new EventListFilter
            {
                OrganizerId = Guid.Parse(User.FindFirst("userId").Value),
                OnlyActive = onlyActive,
                OnlyApproved = onlyApproved,
                CategoryId = category,
                Ordering = (EventListOrder?) orderValue
            };

            var model = _eventService.GetPage(page, filter);

            if (page < 1 || page > model.Page.Total)
                return NotFound();

            return View(model);
        }

        [Route("[controller]/[action]")]
        public IActionResult Statistics()
        {
            var filter = new EventListFilter
            {
                OrganizerId = Guid.Parse(User.FindFirst("userId").Value)
            };

            var events = _eventService.GetList(filter);

            return View(events);
        }

        [HttpGet("[controller]/[action]")]
        public IActionResult AddEvent(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            var model = _eventService.ActualizeModel();
            model.Date = DateTime.Now.Date;


            return View(model);
        }

        [HttpGet("[controller]/[action]/{id:int}")]
        public IActionResult EditEvent(int id, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            var model = _eventService.GetSingle(id);

            if (model == null || model.OrganizerId != Guid.Parse(User.FindFirst("userId").Value))
                return NotFound();

            return View(model);
        }

        [HttpPost("[controller]/[action]")]
        public IActionResult SaveEvent(EventEditViewModel model, string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (model.Date.Date < DateTime.Now.Date)
                ModelState.AddModelError("Date", "Please provide an actual date");

            if (string.IsNullOrWhiteSpace(model.Name))
                ModelState.AddModelError("Name", "Name is required");

            if (!ModelState.IsValid)
            {
                model = _eventService.ActualizeModel(model);
                return View(model.Id.HasValue ? "EditEvent" : "AddEvent", model);
            }

            model.OrganizerId = Guid.Parse(User.FindFirst("userId").Value);

            var ok = true;
            if (model.Id.HasValue)
            {
                ok = _eventService.UpdateEvent(model);
                if (ok) 
                    _logger.Here().Information("Event updated: {@Event}", model);
            }
            else
            {
                _eventService.AddEvent(model);
                _logger.Here().Information("New event created: {@Event}", model);
            }

            if (!ok)
                return NotFound();

            if (string.IsNullOrEmpty(returnUrl))
                return RedirectToAction("Events");

            return Redirect(returnUrl);
        }

        [HttpPost("[controller]/[action]")]
        public IActionResult SavePrice(int id, EventPriceViewModel currentPrice, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            var model = _eventService.GetSingle(id);
            if (model == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                model.CurrentPrice = currentPrice;
                return View("EditEvent", model);
            }

            var ok = true;
            if (currentPrice.Id.HasValue)
                ok = _eventService.UpdatePrice(id, currentPrice);
            else
                _eventService.AddPrice(id, currentPrice);

            if (!ok)
                return NotFound();

            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);

            return View("EditEvent", _eventService.GetSingle(id));
        }

        [HttpPost("[controller]/[action]")]
        public IActionResult RemovePrice(int id, int priceId, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            var model = _eventService.GetSingle(id);
            if (model == null)
                return NotFound();

            if (!_eventService.RemovePrice(priceId))
                return NotFound();

            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);

            model = _eventService.GetSingle(id);
            return View("EditEvent", model);
        }

        [Route("[controller]/{id:guid}")]
        public IActionResult ConfirmTicket(Guid id)
        {
            var model = _eventService.ApproveTicket(id);

            var userId = Guid.Parse(User.FindFirst("userId").Value);
            if (model.EventPrice.Event.OrganizerId != userId)
                model = new TicketApproveViewModel {Approved = false};

            return View("TicketApproved", model);
        }
    }
}
