using System.Data.Entity;
using GoFlex.Core;

namespace GoFlex.Infrastructure
{
    internal abstract class Repository<TEntity> where TEntity : Entity
    {
        protected readonly GoFlexContext context;
        protected readonly DbSet<TEntity> dbSet;

        protected Repository(GoFlexContext context)
        {
            this.context = context;
            dbSet = context.Set<TEntity>();
        }
    }
}
