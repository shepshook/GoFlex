using GoFlex.Core.Entities;

namespace GoFlex.Web.ViewModels
{
    public class OrderViewModel
    {
        public Order Order { get; set; }
        public string StripePublicKey { get; set; }
    }
}
