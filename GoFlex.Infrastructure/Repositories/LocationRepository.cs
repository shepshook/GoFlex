using System.Collections.Generic;
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
            return _database.ReadEntitiesAsync<Location>("usp_LocationSelectList", parameters);
        }

        public Task<Location> UpdateAsync(Location entity)
        {
            return _database.UpdateEntityAsync("usp_LocationUpdate", entity);
        }

        public async Task<Location> InsertAsync(Location entity)
        {
            if (entity.Id != default && await GetAsync(entity.Id) != null)
            {
                return await UpdateAsync(entity);
            }

            return await _database.CreateEntityAsync("usp_LocationInsert", entity);
        }

        public async Task RemoveAsync(int key)
        {
            await _database.RemoveEntityAsync("usp_LocationDelete", key);
        }
    }
}
