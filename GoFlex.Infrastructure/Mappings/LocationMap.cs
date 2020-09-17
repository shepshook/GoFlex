using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using GoFlex.Core.Entities;

namespace GoFlex.Infrastructure.Mappings
{
    public class LocationMap : EntityTypeConfiguration<Location>
    {
        public LocationMap()
        {
            ToTable("Location");

            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("LocationId")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Name).IsRequired().HasMaxLength(256);
            Property(x => x.Address).IsRequired().HasMaxLength(512);
            Property(x => x.Photo).IsOptional();
        }
    }
}
