using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Linq;
using System.Linq.Expressions;
using GoFlex.Core.Entities;
using GoFlex.Core.Repositories;

namespace GoFlex.Infrastructure.Repositories
{
    internal class OrderItemSecretRepository : Repository<OrderItemSecret>, IOrderItemSecretRepository
    {
        public OrderItemSecretRepository(GoFlexContext context) : base(context)
        {
        }

        public OrderItemSecret Get(Guid key) => MakeInclusions().Single(x => x.Id == key);

        public IEnumerable<OrderItemSecret> All(params Expression<Func<OrderItemSecret, bool>>[] predicates)
        {
            var query = MakeInclusions().AsQueryable().ApplyPredicates(predicates);

            return query.ToList();
        }

        public void Insert(OrderItemSecret entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            dbSet.Add(entity);
        }

        public void Remove(Guid key) => dbSet.Remove(dbSet.Find(key) ?? throw new ObjectNotFoundException());

        private IQueryable<OrderItemSecret> MakeInclusions() => dbSet.Include(x => x.OrderItem.EventPrice.Event);
    }
}
