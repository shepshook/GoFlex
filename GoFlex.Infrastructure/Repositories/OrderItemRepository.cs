using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoFlex.Core.Entities;
using GoFlex.Core.Repositories;

namespace GoFlex.Infrastructure.Repositories
{
    public class OrderItemRepository : Repository<OrderItem>, IOrderItemRepository
    {
        public OrderItemRepository(Database db, UnitOfWork uow) : base(db, uow)
        {
        }

        public async Task<OrderItem> GetAsync((int, int) key)
        {
            var entity = await _database.ReadEntityAsync<OrderItem>("usp_Order_TicketSelect", key);
            return entity == null ? null : (await MakeInclusionsAsync(new[] { entity })).Single();
        }

        public async Task<IEnumerable<OrderItem>> GetAllAsync(IDictionary<string, object> parameters = null)
        {
            var entities = await _database.ReadEntitiesAsync<OrderItem>("usp_Order_TicketSelectList", parameters);
            return await MakeInclusionsAsync(entities.ToList());
        }

        public Task<OrderItem> UpdateAsync(OrderItem entity)
        {
            return _database.UpdateEntityAsync("usp_Order_TicketUpdate", entity);
        }

        public async Task<OrderItem> InsertAsync(OrderItem entity)
        {
            if (entity.Id != default && await GetAsync(entity.Id) != null)
            {
                return await UpdateAsync(entity);
            }

            return await _database.CreateEntityAsync("usp_Order_TicketInsert", entity);
        }

        public async Task RemoveAsync((int, int) key)
        {
            await _database.RemoveEntityAsync("usp_Order_TicketDelete", key);
        }

        private async Task<IEnumerable<OrderItem>> MakeInclusionsAsync(IList<OrderItem> entities)
        {
            foreach (var orderItem in entities)
            {
                orderItem.Ticket = await _unitOfWork.TicketRepository.GetAsync(orderItem.TicketId);
            }
            return entities;
        }
    }
}