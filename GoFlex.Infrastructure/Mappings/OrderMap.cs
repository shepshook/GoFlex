using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using GoFlex.Core.Entities;

namespace GoFlex.Infrastructure.Mappings
{
    public class OrderMap : EntityTypeConfiguration<Order>
    {
        public OrderMap()
        {
            ToTable("Order");

            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("OrderId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Timestamp).IsRequired();
            Property(x => x.TotalPrice).IsRequired();

            HasMany(order => order.Items).WithRequired().HasForeignKey(item => item.OrderId);
            HasRequired(order => order.Event).WithMany().HasForeignKey(order => order.EventId);
            HasRequired(order => order.User).WithMany().HasForeignKey(order => order.UserId);
        }
    }
}
