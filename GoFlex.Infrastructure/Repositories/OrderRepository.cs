using System.Collections.Generic;
using System.Linq;
using GoFlex.Core.Entities;
using GoFlex.Core.Repositories;
using System.Threading.Tasks;

namespace GoFlex.Infrastructure.Repositories
{
    internal sealed class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(Database db, UnitOfWork uow) : base(db, uow)
        {
        }

        public async Task<Order> GetAsync(int key)
        {
            var entity = await _database.ReadEntityAsync<Order>("usp_OrderSelect", key);
            return entity == null ? null : (await MakeInclusionsAsync(new[] { entity })).Single();
        }

        public Task<IEnumerable<Order>> GetAllAsync(IDictionary<string, object> parameters = null)
        {
            return _database.ReadEntitiesAsync<Order>("usp_OrderSelectList", parameters);
        }

        public Task<Order> UpdateAsync(Order entity)
        {
            return _database.UpdateEntityAsync("usp_OrderUpdate", entity);
        }

        public async Task<Order> InsertAsync(Order entity)
        {
            if (entity.Id != default && await GetAsync(entity.Id) != null)
            {
                return await UpdateAsync(entity);
            }

            var insertedEntity = await _database.CreateEntityAsync("usp_OrderInsert", entity);

            foreach (var item in entity.Items)
            {
                item.OrderId = insertedEntity.Id;
                await _unitOfWork.OrderItemRepository.InsertAsync(item);
            }

            return insertedEntity;
        }

        public async Task RemoveAsync(int key)
        {
            await _database.RemoveEntityAsync("usp_OrderDelete", key);
        }

        private async Task<IEnumerable<Order>> MakeInclusionsAsync(IList<Order> entities)
        {
            foreach (var order in entities)
            {
                order.User = await _unitOfWork.UserRepository.GetAsync(order.UserId);
                order.Event = await _unitOfWork.EventRepository.GetAsync(order.EventId);
                order.Items = (await _unitOfWork.OrderItemRepository.GetAllAsync()).Where(x => x.OrderId == order.Id).ToList();
            }

            return entities;
        }
    }
}
