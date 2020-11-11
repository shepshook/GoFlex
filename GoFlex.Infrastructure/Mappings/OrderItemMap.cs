using System.Data.Entity.ModelConfiguration;
using GoFlex.Core.Entities;

namespace GoFlex.Infrastructure.Mappings
{
    public class OrderItemMap : EntityTypeConfiguration<OrderItem>
    {
        public OrderItemMap()
        {
            ToTable("OrderItem");

            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("OrderItemId");

            Property(x => x.OrderId).IsRequired();
            Property(x => x.EventPriceId).IsRequired();
            Property(x => x.Quantity).IsRequired();

            HasRequired(x => x.EventPrice).WithMany(x => x.OrderedItems).HasForeignKey(x => x.EventPriceId);
        }
    }
}
