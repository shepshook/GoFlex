using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Linq.Expressions;
using GoFlex.Core.Entities;
using GoFlex.Core.Repositories;

namespace GoFlex.Infrastructure.Repositories
{
    internal sealed class EventRepository : Repository<Event>, IEventRepository
    {
        public EventRepository(GoFlexContext context) : base(context)
        {
        }

        public Event Get(int key) => dbSet.Find(key);

        public IEnumerable<Event> All(Expression<Func<Event, bool>> predicate)
        {
            var query = dbSet.AsQueryable();
            if (predicate != null)
                query = query.Where(predicate);

            return query.ToList();
        }

        public IEnumerable<Event> GetPage<TKey>(int pageSize, int page, out int totalPages,
            Expression<Func<Event, TKey>> order, bool desc = false, params Expression<Func<Event, bool>>[] predicates)
        {
            IQueryable<Event> query = desc ? dbSet.OrderByDescending(order) : dbSet.OrderBy(order);
            query = query.ApplyPredicates(predicates);

            return query.GetPage(pageSize, page, out totalPages);
        }

        public void Insert(Event entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            dbSet.Add(entity);
        }

        public void Remove(int key) => dbSet.Remove(dbSet.Find(key) ?? throw new ObjectNotFoundException());
    }
}
