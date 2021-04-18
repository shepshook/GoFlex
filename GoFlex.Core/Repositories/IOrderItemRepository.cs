using GoFlex.Core.Entities;
using GoFlex.Core.Repositories.Abstractions;

namespace GoFlex.Core.Repositories
{
    public interface IOrderItemRepository : IRepository<OrderItem, (int, int)>
    {   
    }
}