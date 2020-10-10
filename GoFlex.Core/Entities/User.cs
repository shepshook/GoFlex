using System;

namespace GoFlex.Core.Entities
{
    public class User : Entity<Guid>
    {
        public string PhoneNumber { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
    }
}
