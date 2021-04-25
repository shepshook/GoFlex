using GoFlex.Core.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GoFlex.Services.Abstractions
{
    public interface IMailService
    {
        void SendOrder(Order order, string request, IUrlHelper url);
    }
}
