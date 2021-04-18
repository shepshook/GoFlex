using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoFlex.Core.Entities;
using GoFlex.Core.Repositories;

namespace GoFlex.Infrastructure.Repositories
{
    internal sealed class EventRepository : Repository<Event>, IEventRepository
    {

        public EventRepository(Database db, UnitOfWork uow) : base(db, uow)
        {
        }

        public async Task<Event> GetAsync(int key)
        {
            var item = await _database.ReadEntityAsync<Event>("usp_EventSelect", key);
            return (await MakeInclusionsAsync(new[] { item })).Single();
        }

        public async Task<IEnumerable<Event>> GetAllAsync(IDictionary<string, object> parameters)
        {
            var items = await _database.ReadEntitiesAsync<Event>("", parameters);
            return await MakeInclusionsAsync(items.ToList());
        }

        public async Task UpdateAsync(Event entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            foreach (var price in entity.Prices)
                await _unitOfWork.EventPriceRepository.InsertAsync(price);

            await _database.UpdateEntityAsync("usp_EventUpdate", entity);
        }

        public async Task InsertAsync(Event entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (await GetAsync(entity.Id) != null)
            {
                await UpdateAsync(entity);
                return;
            }

            foreach (var price in entity.Prices)
                await _unitOfWork.EventPriceRepository.InsertAsync(price);

            await _database.CreateEntityAsync("usp_EventInsert", entity);
        }

        public async Task RemoveAsync(int key) 
        {
            await _database.RemoveEntityAsync("usp_EventDelete", key);
        }

        private async Task<IEnumerable<Event>> MakeInclusionsAsync(IList<Event> events)
        {
            var prices = await _unitOfWork.EventPriceRepository.GetAllAsync();
            var categories = await _unitOfWork.EventCategoryRepository.GetAllAsync();
            var users = await _unitOfWork.UserRepository.GetAllAsync();
            var locations = await _unitOfWork.LocationRepository.GetAllAsync();

            foreach (var item in events)
            {
                item.Prices = prices.Where(x => x.EventId == item.Id).ToList();
                item.Category = categories.Single(x => x.Id == item.EventCategoryId);
                item.Organizer = users.Single(x => x.Id == item.OrganizerId);
                item.Location = locations.SingleOrDefault(x => x.Id == item.LocationId);
            }

            return events;
        }
    }
}
