using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoFlex.Core.Entities;
using GoFlex.Core.Repositories.Abstractions;
using GoFlex.Services.Abstractions;
using GoFlex.ViewModels;

namespace GoFlex.Services
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        public static int ItemsPerPage { get; } = 12;

        public EventService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Event>> GetList(EventListFilter filter)
        {
            var events = await _unitOfWork.EventRepository.GetAllAsync();

            return filter.ApplyTo(events);
        }

        public async Task<EventListViewModel> GetPage(int page, EventListFilter filter)
        {
            //var events = _unitOfWork.EventRepository.GetPage(ItemsPerPage, page, out var totalPages,
            //    filter.OrderKeySelector, filter.IsDescending, filter.BuildFilters().ToArray());

            var events = filter.ApplyTo(await _unitOfWork.EventRepository.GetAllAsync());
            var pageEvents = events.Skip(ItemsPerPage * (page - 1)).Take(ItemsPerPage);
            var totalPages = (int)Math.Ceiling(events.Count / (double)ItemsPerPage);

            var pageViewModel = new PageViewModel(page, totalPages)
            {
                Parameters = filter.ToDictionary()
            };

            var model = new EventListViewModel
            {
                Events = pageEvents,
                Page = pageViewModel,
                EventCategories = await _unitOfWork.CategoryRepository.GetAllAsync()
            };

            return model;
        }

        public async Task<EventEditViewModel> GetSingle(int id)
        {
            var item = await _unitOfWork.EventRepository.GetAsync(id);
            return await BuildModelFromEntity(item);
        }

        public Task<Event> GetSingleEntity(int id) => _unitOfWork.EventRepository.GetAsync(id);

        public async Task AddEvent(EventEditViewModel model)
        {
            var entity = BuildEntityFromModel(model);

            await _unitOfWork.EventRepository.InsertAsync(entity);
        }

        public async Task<bool> UpdateEvent(EventEditViewModel model)
        {
            var entity = await _unitOfWork.EventRepository.GetAsync(model.Id.Value);
            if (entity == null)
                return false;

            entity = BuildEntityFromModel(model, entity);
            await _unitOfWork.EventRepository.UpdateAsync(entity);

            return true;
        }

        public async Task AddPrice(int id, EventPriceViewModel model)
        {
            var price = new Ticket
            {
                Name = model.Name,
                TotalCount = model.Total,
                Price = model.Price,
                EventId = id
            };

            await _unitOfWork.TicketRepository.InsertAsync(price);
        }

        public async Task<bool> UpdatePrice(int id, EventPriceViewModel model)
        {
            var price = await _unitOfWork.TicketRepository.GetAsync(model.Id.Value);
            if (price == null)
                return false;

            price.Name = model.Name;

            if (model.Total < price.Sold())
                return false;
            price.TotalCount = model.Total;

            if (model.Price != price.Price)
            {
                var newPrice = new Ticket
                {
                    Name = model.Name,
                    TotalCount = price.TotalCount - price.Sold(),
                    Price = model.Price,
                    EventId = price.EventId
                };
                await _unitOfWork.TicketRepository.InsertAsync(newPrice);

                price.TotalCount = price.Sold();
                price.IsRemoved = true;
            }

            await _unitOfWork.TicketRepository.UpdateAsync(price);
            return true;
        }

        public async Task<EventEditViewModel> ActualizeModel(EventEditViewModel model = null)
        {
            model ??= new EventEditViewModel();

            if (model.Id.HasValue)
            {
                model.Prices = (await _unitOfWork.EventRepository.GetAsync(model.Id.Value)).Tickets.Select(price =>
                    new EventPriceViewModel
                    {
                        Id = price.Id,
                        Name = price.Name,
                        Price = price.Price,
                        Total = price.TotalCount,
                        IsRemoved = price.IsRemoved
                    });
            }

            model.Categories = await _unitOfWork.CategoryRepository.GetAllAsync();
            model.Locations = await _unitOfWork.LocationRepository.GetAllAsync();

            return model;
        }

        public async Task<bool> RemovePrice(int priceId)
        {
            var entity = await _unitOfWork.TicketRepository.GetAsync(priceId);
            if (entity == null) 
                return false;

            entity.IsRemoved = true;
            entity.TotalCount = entity.Sold();

            await _unitOfWork.TicketRepository.UpdateAsync(entity);

            return true;
        }

        public async Task<bool> AcceptEvent(int id, bool vote)
        {
            var entity = await _unitOfWork.EventRepository.GetAsync(id);
            if (entity == null)
                return false;

            entity.IsApproved = vote;

            await _unitOfWork.EventRepository.UpdateAsync(entity);

            return true;
        }

        public TicketApproveViewModel ApproveTicket(Guid id)
        {
            //var secret = _unitOfWork.OrderItemSecretRepository.Get(id);
            //if (secret == null || secret.IsUsed)
            //    return new TicketApproveViewModel { Approved = false };

            //secret.IsUsed = true;
            //_unitOfWork.Commit();

            //var eventPrice = _unitOfWork.TicketRepository.Get(secret.OrderItem.EventPriceId);

            //return new TicketApproveViewModel
            //{
            //    Approved = true,
            //    EventPrice = eventPrice
            //};
            throw new NotImplementedException();
        }

        private async Task<EventEditViewModel> BuildModelFromEntity(Event e)
        {
            if (e == null)
                return null;

            var model = new EventEditViewModel
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                CategoryId = e.CategoryId,
                LocationId = e.LocationId,
                OrganizerId = e.OrganizerId,
                Date = e.DateTime.Date,
                Time = e.DateTime.TimeOfDay,
                Photo = e.Photo,
                Prices = e.Tickets.Select(price => new EventPriceViewModel
                {
                    Id = price.Id,
                    Name = price.Name,
                    Price = price.Price,
                    Total = price.TotalCount,
                    IsRemoved = price.IsRemoved
                }),
                Categories = await _unitOfWork.CategoryRepository.GetAllAsync(),
                Locations = await _unitOfWork.LocationRepository.GetAllAsync()
            };
            return model;
        }

        private Event BuildEntityFromModel(EventEditViewModel model, Event entity = null)
        {
            entity ??= new Event();

            entity.Name = model.Name;
            entity.Description = !string.IsNullOrWhiteSpace(model.Description) ? model.Description : null;
            entity.DateTime = model.Date + model.Time;
            entity.CreateTime = DateTime.Now;
            entity.CategoryId = model.CategoryId;
            entity.LocationId = model.LocationId;
            entity.Photo = !string.IsNullOrWhiteSpace(model.Photo) ? model.Photo : null;
            entity.OrganizerId = model.OrganizerId;

            return entity;
        }
    }
}
