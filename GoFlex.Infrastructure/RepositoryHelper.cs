using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace GoFlex.Infrastructure
{
    internal static class RepositoryHelper
    {
        internal static IQueryable<T> ApplyPredicates<T>(this IQueryable<T> source, params Expression<Func<T, bool>>[] predicates)
        {
            if (predicates != null && predicates.Any())
                return predicates
                    .Where(x => x != null)
                    .Aggregate(source, (current, predicate) => current.Where(predicate));

            return source;
        }

        internal static IEnumerable<T> GetPage<T>(this IQueryable<T> query, int pageSize, int page, out int totalPages)
        {
            var count = query.Count();

            totalPages = (int)Math.Ceiling(count / (double)pageSize);
            if (totalPages == 0)
                totalPages = 1;

            if (page < 1 || page > totalPages)
                return new List<T>();

            return query.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        }

        internal static IDictionary<string, object> ToDictionary(this object entity)
        {
            return entity?.GetType().GetProperties()
                .Where(prop => (
                    prop.PropertyType.IsValueType 
                    || prop.PropertyType == typeof(string)) 
                    && !(prop.Name == "Id" 
                        && prop.GetValue(entity).IsDefault())
                    && !prop.PropertyType.IsAssignableTo(typeof(ITuple)))
                .ToDictionary(prop => prop.Name, prop => prop.GetValue(entity));
        }

        private static bool IsDefault(this object value)
        {
            return value switch
            {
                int x => x == 0,
                Guid guid => guid == Guid.Empty,
                _ => false
            };
        }
    }
}
