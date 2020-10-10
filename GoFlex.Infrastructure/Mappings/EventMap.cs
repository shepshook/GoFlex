using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using GoFlex.Core.Entities;

namespace GoFlex.Infrastructure.Mappings
{
    public class EventMap : EntityTypeConfiguration<Event>
    {
        public EventMap()
        {
            ToTable("Event");

            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("EventId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Name).IsRequired().HasMaxLength(128);
            Property(x => x.Description).IsOptional().HasMaxLength(1024);
            Property(x => x.DateTime).IsRequired();
            Property(x => x.Photo).IsOptional();

            HasRequired(x => x.EventCategory).WithMany().HasForeignKey(x => x.EventCategoryId);
            HasRequired(x => x.Organizer).WithMany().HasForeignKey(x => x.OrganizerId);
            HasRequired(x => x.Location).WithMany().HasForeignKey(x => x.LocationId);
            HasMany(e => e.Prices).WithRequired(price => price.Event).HasForeignKey(price => price.EventId);
        }
    }
}
