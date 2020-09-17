using System.Data.Entity.ModelConfiguration;
using GoFlex.Core.Entities;

namespace GoFlex.Infrastructure.Mappings
{
    public class OrderItemMap : EntityTypeConfiguration<OrderItem>
    {
        public OrderItemMap()
        {
            ToTable("OrderItem");

            HasKey(x => new {x.OrderId, x.EventPriceId});

            Property(x => x.OrderId).IsRequired();
            Property(x => x.EventPriceId).IsRequired();
            Property(x => x.Quantity).IsRequired();

            HasRequired(x => x.EventPrice).WithMany().HasForeignKey(x => x.EventPriceId);
        }
    }
}
