using System;

namespace GoFlex.Core.Entities
{
    public class User : Entity<Guid>
    {
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public Role Role { get; set; }
    }

    public enum Role
    {
        Admin,
        Customer,
        Organizer
    }
}
