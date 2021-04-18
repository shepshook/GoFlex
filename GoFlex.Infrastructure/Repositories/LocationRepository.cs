using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GoFlex.Core.Entities;
using GoFlex.Core.Repositories;

namespace GoFlex.Infrastructure.Repositories
{
    internal sealed class LocationRepository : Repository<Location>, ILocationRepository
    {
        public LocationRepository(Database db, UnitOfWork uow) : base(db, uow)
        {
        }

        public Task<Location> GetAsync(int key)
        {
            return _database.ReadEntityAsync<Location>("usp_LocationSelect", key);
        }

        public Task<IEnumerable<Location>> GetAllAsync(IDictionary<string, object> parameters)
        {
            return _database.ReadEntitiesAsync<Location>("", parameters);
        }

        public async Task UpdateAsync(Location entity)
        {
            await _database.UpdateEntityAsync("usp_LocationUpdate", entity);
        }

        public async Task InsertAsync(Location entity)
        {
            if (await GetAsync(entity.Id) != null)
                await UpdateAsync(entity);

            await _database.CreateEntityAsync("usp_LocationInsert", entity);
        }

        public async Task RemoveAsync(int key)
        {
            await _database.RemoveEntityAsync("usp_LocationDelete", key);
        }
    }
}
