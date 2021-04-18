using System;
using GoFlex.Core.Entities;

namespace GoFlex.Core
{
    public class Organizer : Entity<Guid>
    {
        public User User { get; set; }

        public string CompanyName { get; set; }

        public string BankAccountNumber { get; set; }
    }
}