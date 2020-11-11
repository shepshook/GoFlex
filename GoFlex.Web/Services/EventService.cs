using System;
using System.Collections.Generic;
using System.Linq;
using GoFlex.Core.Entities;
using GoFlex.Core.Repositories.Abstractions;
using GoFlex.ViewModels;
using GoFlex.Web.Services.Abstractions;
using GoFlex.Web.ViewModels;

namespace GoFlex.Web.Services
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        public static int ItemsPerPage { get; } = 12;

        public EventService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Event> GetList(EventListFilter filter)
        {
            return _unitOfWork.EventRepository.All(filter.BuildFilters().ToArray());
        }

        public EventListViewModel GetPage(int page, EventListFilter filter)
        {
            var events = _unitOfWork.EventRepository.GetPage(ItemsPerPage, page, out var totalPages,
                filter.OrderKeySelector, filter.IsDescending, filter.BuildFilters().ToArray());

            var pageViewModel = new PageViewModel(page, totalPages)
            {
                Parameters = filter.ToDictionary()
            };

            var model = new EventListViewModel
            {
                Events = events,
                Page = pageViewModel,
                EventCategories = _unitOfWork.EventCategoryRepository.All()
            };

            return model;
        }

        public EventEditViewModel GetSingle(int id)
        {
            var item = _unitOfWork.EventRepository.Get(id);
            return BuildModelFromEntity(item);
        }

        public Event GetSingleEntity(int id) => _unitOfWork.EventRepository.Get(id);

        public void AddEvent(EventEditViewModel model)
        {
            var entity = BuildEntityFromModel(model);

            _unitOfWork.EventRepository.Insert(entity);
            _unitOfWork.Commit();
        }

        public bool UpdateEvent(EventEditViewModel model)
        {
            var entity = _unitOfWork.EventRepository.Get(model.Id.Value);
            if (entity == null)
                return false;

            entity = BuildEntityFromModel(model, entity);
            _unitOfWork.Commit();

            return true;
        }

        public void AddPrice(int id, EventPriceViewModel model)
        {
            var price = new EventPrice
            {
                Name = model.Name,
                Total = model.Total,
                Price = model.Price,
                EventId = id
            };

            _unitOfWork.EventPriceRepository.Insert(price);
            _unitOfWork.Commit();
        }

        public bool UpdatePrice(int id, EventPriceViewModel model)
        {
            var price = _unitOfWork.EventPriceRepository.Get(model.Id.Value);
            if (price == null)
                return false;

            price.Name = model.Name;

            if (model.Total < price.Sold)
                return false;
            price.Total = model.Total;

            if (model.Price != price.Price)
            {
                var newPrice = new EventPrice
                {
                    Name = model.Name,
                    Total = price.Total - price.Sold,
                    Price = model.Price,
                    EventId = price.EventId
                };
                _unitOfWork.EventPriceRepository.Insert(newPrice);

                price.Total = price.Sold;
                price.IsRemoved = true;
            }

            _unitOfWork.Commit();
            return true;
        }

        public EventEditViewModel ActualizeModel(EventEditViewModel model = null)
        {
            model ??= new EventEditViewModel();

            if (model.Id.HasValue)
            {
                model.Prices = _unitOfWork.EventRepository.Get(model.Id.Value).Prices.Select(price =>
                    new EventPriceViewModel
                    {
                        Id = price.Id,
                        Name = price.Name,
                        Price = price.Price,
                        Total = price.Total,
                        IsRemoved = price.IsRemoved
                    });
            }

            model.Categories = _unitOfWork.EventCategoryRepository.All();
            model.Locations = _unitOfWork.LocationRepository.All();

            return model;
        }

        public bool RemovePrice(int priceId)
        {
            var entity = _unitOfWork.EventPriceRepository.Get(priceId);
            if (entity == null) 
                return false;

            entity.IsRemoved = true;
            entity.Total = entity.Sold;
            _unitOfWork.Commit();

            return true;
        }

        public bool AcceptEvent(int id, bool vote)
        {
            var entity = _unitOfWork.EventRepository.Get(id);
            if (entity == null)
                return false;

            entity.IsApproved = vote;
            _unitOfWork.Commit();

            return true;
        }

        public TicketApproveViewModel ApproveTicket(Guid id)
        {
            var secret = _unitOfWork.OrderItemSecretRepository.Get(id);
            if (secret == null || secret.IsUsed)
                return new TicketApproveViewModel {Approved = false};

            secret.IsUsed = true;
            _unitOfWork.Commit();

            var eventPrice = _unitOfWork.EventPriceRepository.Get(secret.OrderItem.EventPriceId);

            return new TicketApproveViewModel
            {
                Approved = true,
                EventPrice = eventPrice
            };
        }

        private EventEditViewModel BuildModelFromEntity(Event e)
        {
            if (e == null)
                return null;

            var model = new EventEditViewModel
            {
                Id = e.Id,
                Name = e.Name,
                Description = e.Description,
                CategoryId = e.EventCategoryId,
                LocationId = e.LocationId,
                OrganizerId = e.OrganizerId,
                Date = e.DateTime.Date,
                Time = e.DateTime.TimeOfDay,
                Photo = e.Photo,
                Prices = e.Prices.Select(price => new EventPriceViewModel
                {
                    Id = price.Id,
                    Name = price.Name,
                    Price = price.Price,
                    Total = price.Total,
                    IsRemoved = price.IsRemoved
                }),
                Categories = _unitOfWork.EventCategoryRepository.All(),
                Locations = _unitOfWork.LocationRepository.All()
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
            entity.EventCategoryId = model.CategoryId;
            entity.LocationId = model.LocationId;
            entity.Photo = !string.IsNullOrWhiteSpace(model.Photo) ? model.Photo : null;
            entity.OrganizerId = model.OrganizerId;

            return entity;
        }
    }
}
