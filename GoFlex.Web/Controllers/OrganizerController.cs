using GoFlex.Core.Repositories.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace GoFlex.Web.Controllers
{
    public class OrganizerController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrganizerController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Route("[controller]/[action]")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
