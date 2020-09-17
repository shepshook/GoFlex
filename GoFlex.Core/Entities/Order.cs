using System;
using System.Collections.Generic;
using System.Linq;

namespace GoFlex.Core.Entities
{
    public class Order : Entity<int>
    {
        public Guid UserId { get; set; }
        public int EventId { get; set; }
        public DateTime Timestamp { get; set; }

        public User User { get; set; }
        public Event Event { get; set; }
        public ICollection<OrderItem> Items { get; set; }

        public decimal TotalPrice => Items.Sum(x => x.EventPrice.Price * x.Quantity);
    }
}
