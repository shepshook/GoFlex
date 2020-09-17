using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using GoFlex.Core.Entities;

namespace GoFlex.Infrastructure.Mappings
{
    public class RoleMap : EntityTypeConfiguration<Role>
    {
        public RoleMap()
        {
            ToTable("Role");

            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("RoleId").HasDatabaseGeneratedOption(DatabaseGeneratedOption.Identity);

            Property(x => x.Name).IsRequired().HasMaxLength(64);
        }
    }
}
