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

        public virtual User User { get; set; }
        public virtual Event Event { get; set; }
        public virtual ICollection<OrderItem> Items { get; set; }

        public decimal TotalPrice() => Items?.Sum(x => x.Ticket.Price * x.Quantity) ?? 0;
        public string TotalPriceFormatted() => $"{TotalPrice():N} BYN";
    }
}
