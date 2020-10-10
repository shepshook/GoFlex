using System.Data.Entity.ModelConfiguration;
using GoFlex.Core.Entities;

namespace GoFlex.Infrastructure.Mappings
{
    public class CityMap : EntityTypeConfiguration<City>
    {
        public CityMap()
        {
            ToTable("City");

            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("CityId");

            Property(x => x.Name).IsRequired().HasMaxLength(64);
        }
    }
}
