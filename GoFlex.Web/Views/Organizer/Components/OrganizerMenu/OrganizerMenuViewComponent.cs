using Microsoft.AspNetCore.Mvc;

namespace GoFlex.Web.Components
{
    public class OrganizerMenuViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int activeTab)
        {
            return View("Default", activeTab);
        }
    }
}
