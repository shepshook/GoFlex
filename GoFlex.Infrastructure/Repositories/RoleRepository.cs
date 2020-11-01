using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Linq.Expressions;
using GoFlex.Core.Entities;
using GoFlex.Core.Repositories;

namespace GoFlex.Infrastructure.Repositories
{
    internal sealed class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(GoFlexContext context) : base(context)
        {
        }

        public Role Get(int key) => dbSet.Find(key);

        public IEnumerable<Role> All(params Expression<Func<Role, bool>>[] predicates)
        {
            var query = dbSet.AsQueryable().ApplyPredicates(predicates);

            return query.ToList();
        }

        public void Insert(Role entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            dbSet.Add(entity);
        }

        public void Remove(int key)
        {
            dbSet.Remove(dbSet.Find(key) ?? throw new ObjectNotFoundException());
        }
    }
}
