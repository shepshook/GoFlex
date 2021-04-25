using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<User> GetByEmailAsync(string email)
        {
            return (await GetAllAsync()).SingleOrDefault(x => string.Equals(x.Email, email, StringComparison.InvariantCultureIgnoreCase));
        }

        public Task<IEnumerable<User>> GetAllAsync(IDictionary<string, object> parameters = null)
        {
            return _database.ReadEntitiesAsync<User>("usp_UserSelectList", parameters);
        }

        public Task<User> UpdateAsync(User entity)
        {
            return _database.UpdateEntityAsync("usp_UserUpdate", entity);
        }

        public async Task<User> InsertAsync(User entity)
        {
            if (entity.Id == default)
            {
                entity.Id = Guid.NewGuid();
            }
            else if (await GetAsync(entity.Id) != null)
            {
                return await UpdateAsync(entity);
            }

            return await _database.CreateEntityAsync("usp_UserInsert", entity);
        }

        public async Task RemoveAsync(Guid key)
        {
            await _database.RemoveEntityAsync("usp_UserDelete", key);
        }
    }
}
