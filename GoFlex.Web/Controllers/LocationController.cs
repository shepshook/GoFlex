using GoFlex.Core.Repositories.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace GoFlex.Web.Controllers
{
    public class LocationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public LocationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Route("[controller]/[action]")]
        public IActionResult List()
        {
            var list = _unitOfWork.LocationRepository.All();
            return View(list);
        }
    }
}
