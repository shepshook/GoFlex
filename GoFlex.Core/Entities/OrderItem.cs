namespace GoFlex.Core.Entities
{
    public class OrderItem : Entity
    {
        public int OrderId { get; set; }
        public int EventPriceId { get; set; }
        public int Quantity { get; set; }

        public virtual EventPrice EventPrice { get; set; }
    }
}
