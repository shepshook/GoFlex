using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GoFlex.Core.Entities;
using GoFlex.Core.Repositories;

namespace GoFlex.Infrastructure.Repositories
{
    internal sealed class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(Database db, UnitOfWork uow) : base(db, uow)
        {
        }

        public Task<Role> GetAsync(int key)
        {
            return _database.ReadEntityAsync<Role>("usp_RoleSelect", key);
        }

        public Task<IEnumerable<Role>> GetAllAsync(IDictionary<string, object> parameters = null)
        {
            return _database.ReadEntitiesAsync<Role>("", parameters);
        }

        public async Task UpdateAsync(Role entity)
        {
            await _database.UpdateEntityAsync("usp_RoleUpdate", entity);
        }

        public async Task InsertAsync(Role entity)
        {
            if (await GetAsync(entity.Id) != null)
                await UpdateAsync(entity);

            await _database.CreateEntityAsync("usp_RoleInsert", entity);
        }

        public async Task RemoveAsync(int key)
        {
            await _database.RemoveEntityAsync("usp_RoleDelete", key);
        }
    }
}
