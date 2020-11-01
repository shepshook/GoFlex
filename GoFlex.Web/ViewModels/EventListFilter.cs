using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GoFlex.Core.Entities;

namespace GoFlex.Web.ViewModels
{
    public class EventListFilter
    {
        public int? CategoryId { get; set; }
        public bool? OnlyActive { get; set; }
        public bool? OnlyApproved { get; set; }
        public Guid? OrganizerId { get; set; }
        public EventListOrder? Ordering { get; set; }
        public IEnumerable<Expression<Func<Event, bool>>> AdditionalFilters { get; set; }

        public Expression<Func<Event, DateTime>> OrderKeySelector => Ordering switch
        {
            EventListOrder.CreateDate => x => x.CreateTime,
            EventListOrder.Date => x => x.DateTime,
            _ => x => x.DateTime
        };

        public bool IsDescending => Ordering switch
        {
            EventListOrder.CreateDate => true,
            EventListOrder.Date => false,
            _ => false
        };

        public IEnumerable<Expression<Func<Event, bool>>> BuildFilters()
        {
            var filters = new List<Expression<Func<Event, bool>>>();

            if (CategoryId.HasValue)
                filters.Add(x => x.EventCategoryId == CategoryId);

            if (OnlyActive.HasValue && OnlyActive.Value)
                filters.Add(x => x.DateTime > DateTime.Now);

            if (OnlyApproved.HasValue)
                filters.Add(x => x.IsApproved == OnlyApproved.Value);

            if (OrganizerId.HasValue)
                filters.Add(x => x.OrganizerId == OrganizerId.Value);

            if (AdditionalFilters != null && AdditionalFilters.Any())
                filters.AddRange(AdditionalFilters);

            return filters;
        }

        public IDictionary<string, object> ToDictionary()
        {
            var result = new Dictionary<string, object>();

            if (CategoryId.HasValue)
                result.Add("category", CategoryId.Value);

            if (Ordering.HasValue)
                result.Add("order", Ordering.Value);

            if (OnlyActive.HasValue)
                result.Add("onlyActive", OnlyActive.Value);

            if (OnlyApproved.HasValue)
                result.Add("onlyApproved", OnlyApproved.Value);

            return result;
        }
    }
}
