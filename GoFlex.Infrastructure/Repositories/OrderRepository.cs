using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Linq.Expressions;
using GoFlex.Core.Entities;
using GoFlex.Core.Repositories;

namespace GoFlex.Infrastructure.Repositories
{
    internal sealed class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(GoFlexContext context) : base(context)
        {
        }

        public Order Get(int key) => dbSet.Find(key);

        public IEnumerable<Order> All(Expression<Func<Order, bool>> predicate)
        {
            var query = dbSet.AsQueryable();
            if (predicate != null)
                query = query.Where(predicate);

            return query.ToList();
        }

        public void Insert(Order entity)
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
