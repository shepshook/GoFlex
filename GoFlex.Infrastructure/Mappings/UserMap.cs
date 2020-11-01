using System.Data.Entity.ModelConfiguration;
using GoFlex.Core.Entities;

namespace GoFlex.Infrastructure.Mappings
{
    public class UserMap : EntityTypeConfiguration<User>
    {
        public UserMap()
        {
            ToTable("User");

            HasKey(x => x.Id);
            Property(x => x.Id).HasColumnName("UserId");

            Property(x => x.PasswordHash).IsRequired().HasMaxLength(128);
            Property(x => x.PasswordSalt).IsRequired().HasMaxLength(128);
            Property(x => x.Email).IsRequired().HasMaxLength(64);

            HasRequired(user => user.Role).WithMany().HasForeignKey(user => user.RoleId);
        }
    }
}
