using GoFlex.Core.Repositories.Abstractions;
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
        public IActionResult List()
        {
            var list = _unitOfWork.EventRepository.All();
            return View(list);
        }

        [Route("[controller]/[action]/{id:int}")]
        public IActionResult Details(int id)
        {
            var item = _unitOfWork.EventRepository.Get(id);
            return View(item);
        }
    }
}
