using System.Collections.Generic;

namespace GoFlex.Web.ViewModels
{
    public class PageViewModel
    {
        public int Current { get; set; }
        public int Total { get; set; }

        public IDictionary<string, object> Parameters { get; set; }

        public PageViewModel(int current, int total)
        {
            Current = current;
            Total = total;
            Parameters = new Dictionary<string, object>();
        }

        public bool HasNext => Current < Total;

        public bool HasPrevious => Current > 1;
    }
}