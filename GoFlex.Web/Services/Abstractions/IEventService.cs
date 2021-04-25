using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoFlex.Core.Entities;
using GoFlex.ViewModels;

namespace GoFlex.Services.Abstractions
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetList(EventListFilter filter);

        Task<EventListViewModel> GetPage(int page, EventListFilter filter);

        Task<EventEditViewModel> GetSingle(int id);

        Task<Event> GetSingleEntity(int id);

        Task AddEvent(EventEditViewModel model);

        Task<bool> UpdateEvent(EventEditViewModel model);

        Task AddPrice(int id, EventPriceViewModel model);

        Task<bool> UpdatePrice(int id, EventPriceViewModel model);

        Task<bool> RemovePrice(int priceId);

        Task<EventEditViewModel> ActualizeModel(EventEditViewModel model = null);

        Task<bool> AcceptEvent(int id, bool vote);

        TicketApproveViewModel ApproveTicket(Guid id);
    }
}
