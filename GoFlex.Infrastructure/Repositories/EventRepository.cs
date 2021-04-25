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
            var items = await _database.ReadEntitiesAsync<Event>("usp_EventSelectList", parameters);
            return await MakeInclusionsAsync(items.ToList());
        }

        public async Task<Event> UpdateAsync(Event entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            foreach (var price in entity.Tickets)
                await _unitOfWork.TicketRepository.InsertAsync(price);

            return await _database.UpdateEntityAsync("usp_EventUpdate", entity);
        }

        public async Task<Event> InsertAsync(Event entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            if (entity.Id != default && await GetAsync(entity.Id) != null)
            {
                return await UpdateAsync(entity);
            }

            var insertedEntity = await _database.CreateEntityAsync("usp_EventInsert", entity);

            if (entity.Tickets != null)
            {
                foreach (var ticket in entity.Tickets)
                {
                    ticket.EventId = insertedEntity.Id;
                    await _unitOfWork.TicketRepository.InsertAsync(ticket);
                }
            }

            return insertedEntity;
        }

        public async Task RemoveAsync(int key)
        {
            await _database.RemoveEntityAsync("usp_EventDelete", key);
        }

        private async Task<IEnumerable<Event>> MakeInclusionsAsync(IList<Event> events)
        {
            var prices = (await _unitOfWork.TicketRepository.GetAllAsync()).ToList();
            var categories = (await _unitOfWork.CategoryRepository.GetAllAsync()).ToList();
            var users = (await _unitOfWork.UserRepository.GetAllAsync()).ToList();
            var locations = (await _unitOfWork.LocationRepository.GetAllAsync()).ToList();

            foreach (var item in events.Where(x => x != null))
            {
                item.Tickets = prices.Where(x => x?.EventId == item.Id).ToList();
                item.Category = categories.Single(x => x?.Id == item.CategoryId);
                item.Organizer = users.Single(x => x?.Id == item.OrganizerId);
                item.Location = locations.SingleOrDefault(x => x?.Id == item.LocationId);
            }

            return events;
        }
    }
}
