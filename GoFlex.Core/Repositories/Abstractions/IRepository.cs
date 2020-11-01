using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GoFlex.Core.Repositories.Abstractions
{
    public interface IRepository<TEntity, in TKey> where TEntity : Entity<TKey>
    {
        TEntity Get(TKey key);
        IEnumerable<TEntity> All(params Expression<Func<TEntity, bool>>[] predicates);
        void Insert(TEntity entity);
        void Remove(TKey key);
    }
}
