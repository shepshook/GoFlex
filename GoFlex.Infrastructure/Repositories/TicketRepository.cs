using System.Collections.Generic;
using System.Threading.Tasks;
using GoFlex.Core.Entities;
using GoFlex.Core.Repositories;

namespace GoFlex.Infrastructure.Repositories
{
    internal sealed class TicketRepository : Repository<Ticket>, ITicketRepository
    {
        public TicketRepository(Database db, UnitOfWork uow) : base(db, uow)
        {
        }

        // public IEnumerable<EventPrice> GetAllAsync(params Expression<Func<EventPrice, bool>>[] predicates)
        // {
        //     var query = dbSet.OrderByDescending(x => x.IsRemoved).ThenBy(x => x.Price).AsQueryable().ApplyPredicates(predicates);

        //     return query.ToList();
        // }

        public Task<Ticket> GetAsync(int key)
        {
            return _database.ReadEntityAsync<Ticket>("usp_TicketSelect", key); 
        }

        public Task<IEnumerable<Ticket>> GetAllAsync(IDictionary<string, object> parameters)
        {
            return _database.ReadEntitiesAsync<Ticket>("", parameters);
        }

        public async Task UpdateAsync(Ticket entity)
        {
            await _database.UpdateEntityAsync("usp_TicketUpdate", entity);
        }

        public async Task InsertAsync(Ticket entity)
        {
            if (await GetAsync(entity.Id) != null)
                await UpdateAsync(entity);

            await _database.CreateEntityAsync("usp_TicketInsert", entity);
        }

        public async Task RemoveAsync(int key)
        {
            await _database.RemoveEntityAsync("usp_TicketDelete", key);
        }
    }
}
