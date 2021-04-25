using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoFlex.Core;
using GoFlex.Core.Entities;
using GoFlex.Core.Repositories;

namespace GoFlex.Infrastructure.Repositories
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(Database db, UnitOfWork uow) : base(db, uow)
        {
        }

        public Task<Comment> GetAsync(Guid key)
        {
            return _database.ReadEntityAsync<Comment>("usp_CommentSelect", key);
        }

        public Task<IEnumerable<Comment>> GetAllAsync(IDictionary<string, object> parameters = null)
        {
            return _database.ReadEntitiesAsync<Comment>("usp_CommentSelectList", parameters);
        }

        public Task<Comment> UpdateAsync(Comment entity)
        {
            return _database.UpdateEntityAsync("usp_CommentUpdate", entity);
        }

        public async Task<Comment> InsertAsync(Comment entity)
        {
            if (entity.Id == default)
            {
                entity.Id = Guid.NewGuid();
            }
            else if (await GetAsync(entity.Id) != null)
            {
                return await UpdateAsync(entity);
            }

            return await _database.CreateEntityAsync("usp_CommentInsert", entity);
        }

        public async Task RemoveAsync(Guid key)
        {
            await _database.RemoveEntityAsync("usp_CommentDelete", key);
        }
    }
}