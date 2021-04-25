using System.Collections.Generic;
using GoFlex.Core.Entities;

namespace GoFlex.ViewModels
{
    public class EventListViewModel
    {
        public IEnumerable<Event> Events { get; set; }
        public PageViewModel Page { get; set; }

        public IEnumerable<Category> EventCategories { get; set; }
    }
}
