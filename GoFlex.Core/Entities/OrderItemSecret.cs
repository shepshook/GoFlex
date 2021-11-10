using System;

namespace GoFlex.Core.Entities
{
    public class OrderItemSecret : Entity<Guid>
    {
        public bool IsUsed { get; set; }

        public int OrderItemId { get; set; }

        public virtual OrderItem OrderItem { get; set; }
    }
}
