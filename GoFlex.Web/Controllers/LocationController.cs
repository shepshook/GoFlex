using System.Threading.Tasks;
using GoFlex.Core.Repositories.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace GoFlex.Controllers
{
    public class LocationController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public LocationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Route("[controller]/[action]")]
        public async Task<IActionResult> List()
        {
            var list = await _unitOfWork.LocationRepository.GetAllAsync();
            return View(list);
        }
    }
}
