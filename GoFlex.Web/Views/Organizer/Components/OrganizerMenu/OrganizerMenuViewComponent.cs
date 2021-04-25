using Microsoft.AspNetCore.Mvc;

namespace GoFlex.Views.Organizer.Components.OrganizerMenu
{
    public class OrganizerMenuViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(int activeTab)
        {
            return View("Default", activeTab);
        }
    }
}
