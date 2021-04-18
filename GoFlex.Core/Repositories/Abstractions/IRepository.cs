using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoFlex.Core.Repositories.Abstractions
{
    public interface IRepository<TEntity, in TKey> where TEntity : Entity<TKey>
    {
        Task<TEntity> GetAsync(TKey key);
        Task<IEnumerable<TEntity>> GetAllAsync(IDictionary<string, object> parameters = null);
        Task UpdateAsync(TEntity entity);
        Task InsertAsync(TEntity entity);
        Task RemoveAsync(TKey key);
    }
}
