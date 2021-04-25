using GoFlex.Core.Entities;

namespace GoFlex.ViewModels
{
    public class OrderViewModel
    {
        public Order Order { get; set; }
        public string StripePublicKey { get; set; }
    }
}
