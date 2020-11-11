using System.Data.Entity.ModelConfiguration;
using GoFlex.Core.Entities;

namespace GoFlex.Infrastructure.Mappings
{
    public class OrderItemSecretMap : EntityTypeConfiguration<OrderItemSecret>
    {
        public OrderItemSecretMap()
        {
            ToTable("OrderItemSecret");

            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("OrderItemSecretId");

            Property(x => x.IsUsed).IsRequired();

            HasRequired(x => x.OrderItem).WithMany(y => y.Secrets).HasForeignKey(x => x.OrderItemId);
        }
    }
}
