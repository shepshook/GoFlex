using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Linq.Expressions;
using GoFlex.Core.Entities;
using GoFlex.Core.Repositories;
using System.Data.Entity;

namespace GoFlex.Infrastructure.Repositories
{
    internal sealed class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(GoFlexContext context) : base(context)
        {
        }

        public Order Get(int key) => MakeInclusions().SingleOrDefault(x => x.Id == key);

        public IEnumerable<Order> All(params Expression<Func<Order, bool>>[] predicates)
        {
            var query = MakeInclusions().AsQueryable().ApplyPredicates(predicates);

            return query.ToList();
        }

        public void Insert(Order entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            dbSet.Add(entity);
        }

        public void Remove(int key) => dbSet.Remove(dbSet.Find(key) ?? throw new ObjectNotFoundException());

        private IQueryable<Order> MakeInclusions() =>
            dbSet.Include(x => x.User)
                .Include(x => x.Event)
                .Include(x => x.Items.Select(item => item.EventPrice));
    }
}
