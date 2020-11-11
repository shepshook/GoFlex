using System;
using System.Collections.Generic;
using GoFlex.Core.Entities;
using GoFlex.ViewModels;
using GoFlex.Web.ViewModels;

namespace GoFlex.Web.Services.Abstractions
{
    public interface IEventService
    {
        public IEnumerable<Event> GetList(EventListFilter filter);

        public EventListViewModel GetPage(int page, EventListFilter filter);

        public EventEditViewModel GetSingle(int id);

        public Event GetSingleEntity(int id);

        public void AddEvent(EventEditViewModel model);

        public bool UpdateEvent(EventEditViewModel model);

        public void AddPrice(int id, EventPriceViewModel model);

        public bool UpdatePrice(int id, EventPriceViewModel model);

        public bool RemovePrice(int priceId);

        public EventEditViewModel ActualizeModel(EventEditViewModel model = null);

        public bool AcceptEvent(int id, bool vote);

        public TicketApproveViewModel ApproveTicket(Guid id);
    }
}
