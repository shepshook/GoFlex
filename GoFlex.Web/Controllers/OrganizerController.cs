using System;
using System.Threading.Tasks;
using GoFlex.Services.Abstractions;
using GoFlex.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace GoFlex.Controllers
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
        public async Task<IActionResult> Events(int? category, bool? onlyActive, bool? onlyApproved, string order, int page = 1)
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

            var model = await _eventService.GetPage(page, filter);

            if (page < 1 || page > model.Page.Total && model.Page.Total != 0)
                return NotFound();

            return View(model);
        }

        [Route("[controller]/[action]")]
        public async Task<IActionResult> Statistics()
        {
            var filter = new EventListFilter
            {
                OrganizerId = Guid.Parse(User.FindFirst("userId").Value)
            };

            var events = await _eventService.GetList(filter);

            return View(events);
        }

        [HttpGet("[controller]/[action]")]
        public async Task<IActionResult> AddEvent(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            var model = await _eventService.ActualizeModel();
            model.Date = DateTime.Now.Date;

            return View(model);
        }

        [HttpGet("[controller]/[action]/{id:int}")]
        public async Task<IActionResult> EditEvent(int id, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            var model = await _eventService.GetSingle(id);

            if (model == null || model.OrganizerId != Guid.Parse(User.FindFirst("userId").Value))
                return NotFound();

            return View(model);
        }

        [HttpPost("[controller]/[action]")]
        public async Task<IActionResult> SaveEvent(EventEditViewModel model, string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (model.Date.Date < DateTime.Now.Date)
                ModelState.AddModelError("Date", "Please provide an actual date");

            if (string.IsNullOrWhiteSpace(model.Name))
                ModelState.AddModelError("Name", "Name is required");

            if (!ModelState.IsValid)
            {
                model = await _eventService.ActualizeModel(model);
                return View(model.Id.HasValue ? "EditEvent" : "AddEvent", model);
            }

            model.OrganizerId = Guid.Parse(User.FindFirst("userId").Value);

            var ok = true;
            if (model.Id.HasValue)
            {
                ok = await _eventService.UpdateEvent(model);
                if (ok) 
                    _logger.Here().Information("Event updated: {@Event}", model);
            }
            else
            {
                await _eventService.AddEvent(model);
                _logger.Here().Information("New event created: {@Event}", model);
            }

            if (!ok)
                return NotFound();

            if (string.IsNullOrEmpty(returnUrl))
                return RedirectToAction("Events");

            return Redirect(returnUrl);
        }

        [HttpPost("[controller]/[action]")]
        public async Task<IActionResult> SavePrice(int id, EventPriceViewModel currentPrice, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            var model = await _eventService.GetSingle(id);
            if (model == null)
                return NotFound();

            if (!ModelState.IsValid)
            {
                model.CurrentPrice = currentPrice;
                return View("EditEvent", model);
            }

            var ok = true;
            if (currentPrice.Id.HasValue)
                ok = await _eventService.UpdatePrice(id, currentPrice);
            else
                await _eventService.AddPrice(id, currentPrice);

            if (!ok)
                return NotFound();

            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);

            return View("EditEvent", await _eventService.GetSingle(id));
        }

        [HttpPost("[controller]/[action]")]
        public async Task<IActionResult> RemovePrice(int id, int priceId, string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            var model = await _eventService.GetSingle(id);
            if (model == null)
                return NotFound();

            if (!await _eventService.RemovePrice(priceId))
                return NotFound();

            if (!string.IsNullOrEmpty(returnUrl))
                return Redirect(returnUrl);

            model = await _eventService.GetSingle(id);
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
