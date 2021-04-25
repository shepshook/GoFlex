namespace GoFlex.Core.Entities
{
    public class OrderItem : Entity<(int, int)>
    {
        public new(int, int) Id => (OrderId, TicketId);

        public int OrderId { get; set; }
        public int TicketId { get; set; }
        public int Quantity { get; set; }

        public virtual Ticket Ticket { get; set; }
    }
}
