using System.Data.Entity;
using GoFlex.Infrastructure.Mappings;

namespace GoFlex.Infrastructure
{
    public class GoFlexContext : DbContext
    {
        private readonly string _connection;

        public GoFlexContext(string connection) : base(connection)
        {
            _connection = connection;
            Database.SetInitializer<GoFlexContext>(null);
        }

        protected override void OnModelCreating(DbModelBuilder builder)
        {
            builder.Configurations.Add(new EventMap());
            builder.Configurations.Add(new EventCategoryMap());
            builder.Configurations.Add(new EventPriceMap());
            builder.Configurations.Add(new LocationMap());
            builder.Configurations.Add(new OrderMap());
            builder.Configurations.Add(new OrderItemMap());
            builder.Configurations.Add(new RoleMap());
            builder.Configurations.Add(new UserMap());
        }
    }
}
