using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Linq.Expressions;
using GoFlex.Core.Entities;
using GoFlex.Core.Repositories;

namespace GoFlex.Infrastructure.Repositories
{
    internal sealed class EventPriceRepository : Repository<EventPrice>, IEventPriceRepository
    {
        public EventPriceRepository(GoFlexContext context) : base(context)
        {
        }

        public EventPrice Get(int key) => dbSet.Find(key);

        public IEnumerable<EventPrice> All(params Expression<Func<EventPrice, bool>>[] predicates)
        {
            var query = dbSet.OrderByDescending(x => x.IsRemoved).ThenBy(x => x.Price).AsQueryable().ApplyPredicates(predicates);

            return query.ToList();
        }

        public void Insert(EventPrice entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            dbSet.Add(entity);
        }

        public void Remove(int key) => dbSet.Remove(dbSet.Find(key) ?? throw new ObjectNotFoundException());
    }
}
