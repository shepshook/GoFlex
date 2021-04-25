using System.Collections.Generic;
using System.Threading.Tasks;
using GoFlex.Core.Entities;
using GoFlex.Core.Repositories;

namespace GoFlex.Infrastructure.Repositories
{
    internal sealed class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(Database db, UnitOfWork uow) : base(db, uow)
        {
        }

        public Task<Category> GetAsync(int key)
        {
            return _database.ReadEntityAsync<Category>("usp_CategorySelect", key);
        }

        public Task<IEnumerable<Category>> GetAllAsync(IDictionary<string, object> parameters = null)
        {
            return _database.ReadEntitiesAsync<Category>("usp_CategorySelectList", parameters);
        }

        public Task<Category> UpdateAsync(Category entity)
        {
            return _database.UpdateEntityAsync("usp_CategoryUpdate", entity);
        }

        public async Task<Category> InsertAsync(Category entity)
        {
            if (entity.Id != default && await GetAsync(entity.Id) != null)
            {
                return await UpdateAsync(entity);
            }

            return await _database.CreateEntityAsync("usp_CategoryInsert", entity);
        }

        public async Task RemoveAsync(int key)
        {
            await _database.RemoveEntityAsync("usp_CategoryDelete", key);
        }
    }
}
