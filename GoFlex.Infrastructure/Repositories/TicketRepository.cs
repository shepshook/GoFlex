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
            return _database.ReadEntitiesAsync<Ticket>("usp_TicketSelectList", parameters);
        }

        public Task<Ticket> UpdateAsync(Ticket entity)
        {
            return _database.UpdateEntityAsync("usp_TicketUpdate", entity);
        }

        public async Task<Ticket> InsertAsync(Ticket entity)
        {
            if (entity.Id != default && await GetAsync(entity.Id) != null)
            {
                return await UpdateAsync(entity);
            }

            return await _database.CreateEntityAsync("usp_TicketInsert", entity);
        }

        public async Task RemoveAsync(int key)
        {
            await _database.RemoveEntityAsync("usp_TicketDelete", key);
        }
    }
}
