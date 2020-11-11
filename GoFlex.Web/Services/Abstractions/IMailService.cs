using GoFlex.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Session = Stripe.Checkout.Session;

namespace GoFlex.Web.Services.Abstractions
{
    public interface IMailService
    {
        void SendOrder(Order order, string request, IUrlHelper url);
    }
}
