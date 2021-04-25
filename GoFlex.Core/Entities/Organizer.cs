using System;

namespace GoFlex.Core.Entities
{
    public class Organizer : Entity<Guid>
    {
        public User User { get; set; }

        public string CompanyName { get; set; }

        public string BankAccountNumber { get; set; }
    }
}