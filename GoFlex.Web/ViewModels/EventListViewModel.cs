using System.Collections.Generic;
using GoFlex.Core.Entities;

namespace GoFlex.Web.ViewModels
{
    public class EventListViewModel
    {
        public IEnumerable<Event> Events { get; set; }
        public PageViewModel Page { get; set; }

        public IEnumerable<EventCategory> EventCategories { get; set; }
    }
}
