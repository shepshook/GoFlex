using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using GoFlex.Core.Entities;

namespace GoFlex.Infrastructure.Mappings
{
    public class EventPriceMap : EntityTypeConfiguration<EventPrice>
    {
        public EventPriceMap()
        {
            ToTable("EventPrice");

            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("EventPriceId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Name).IsRequired().HasMaxLength(64);
            Property(x => x.Price).IsRequired();
            Property(x => x.Total).IsRequired();
            Property(x => x.IsRemoved).IsRequired();
        }
    }
}
