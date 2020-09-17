using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using GoFlex.Core.Entities;

namespace GoFlex.Infrastructure.Mappings
{
    public class EventCategoryMap : EntityTypeConfiguration<EventCategory>
    {
        public EventCategoryMap()
        {
            ToTable("EventCategory");

            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("EventCategoryId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Name).IsRequired().HasMaxLength(64);
        }
    }
}
