using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace GoFlex.Core.Repositories.Abstractions
{
    public interface IPagedRepository<TEntity> where TEntity : Entity
    {
        IEnumerable<TEntity> GetPage<TOrderKey>(int pageSize, int page, out int totalPages, 
            Expression<Func<TEntity, TOrderKey>> orderKeySelector, bool desc = false, params Expression<Func<TEntity, bool>>[] predicates);
    }
}
