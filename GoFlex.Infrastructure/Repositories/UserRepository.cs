using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Linq.Expressions;
using GoFlex.Core.Entities;
using GoFlex.Core.Repositories;

namespace GoFlex.Infrastructure.Repositories
{
    internal sealed class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(GoFlexContext context) : base(context)
        {
        }

        public User Get(Guid key) => dbSet.Find(key);

        public IEnumerable<User> All(Expression<Func<User, bool>> predicate)
        {
            var query = dbSet.AsQueryable();
            if (predicate != null)
                query = query.Where(predicate);

            return query.ToList();
        }

        public void Insert(User entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            entity.Id = Guid.NewGuid();
            dbSet.Add(entity);
        }

        public void Remove(Guid key) => dbSet.Remove(dbSet.Find(key) ?? throw new ObjectNotFoundException());
    }
}
