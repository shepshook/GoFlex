using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoFlex.Core.Entities;
using GoFlex.Core.Repositories;

namespace GoFlex.Infrastructure.Repositories
{
    internal sealed class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(Database db, UnitOfWork uow) : base(db, uow)
        {
        }

        public Task<User> GetAsync(Guid key)
        {
            return _database.ReadEntityAsync<User>("usp_UserSelect", key);
        }

        public Task<IEnumerable<User>> GetAllAsync(IDictionary<string, object> parameters = null)
        {
            return _database.ReadEntitiesAsync<User>("", parameters);
        }

        public async Task UpdateAsync(User entity)
        {
            await _database.UpdateEntityAsync("usp_UserUpdate", entity);
        }

        public async Task InsertAsync(User entity)
        {
            if (await GetAsync(entity.Id) != null)
                await UpdateAsync(entity);

            await _database.CreateEntityAsync("usp_UserInsert", entity);
        }

        public async Task RemoveAsync(Guid key)
        {
            await _database.RemoveEntityAsync("usp_UserDelete", key);
        }
    }
}
