using System;

namespace GoFlex.Core.Entities
{
    public class Order : Entity<int>
    {
        public Guid UserId { get; set; }
        public int EventId { get; set; }
        public DateTime Timestamp { get; set; }

        public User User { get; set; }
        public Event Event { get; set; }
    }
}
