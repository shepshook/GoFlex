﻿using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Linq;
using System.Linq.Expressions;
using GoFlex.Core.Entities;
using GoFlex.Core.Repositories;

namespace GoFlex.Infrastructure.Repositories
{
    internal sealed class LocationRepository : Repository<Location>, ILocationRepository
    {
        public LocationRepository(GoFlexContext context) : base(context)
        {
        }

        public Location Get(int key) => dbSet.Find(key);

        public IEnumerable<Location> All(Expression<Func<Location, bool>> predicate)
        {
            var query = dbSet.AsQueryable();
            if (predicate != null)
                query = query.Where(predicate);

            return query.ToList();
        }

        public void Insert(Location entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            dbSet.Add(entity);
        }

        public void Remove(int key) => dbSet.Remove(dbSet.Find(key) ?? throw new ObjectNotFoundException());
    }
}
