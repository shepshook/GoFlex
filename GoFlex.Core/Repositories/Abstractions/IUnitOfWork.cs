using System;

namespace GoFlex.Core.Repositories.Abstractions
{
    public interface IUnitOfWork : IDisposable
    {
        IEventRepository EventRepository { get; }
        IEventCategoryRepository EventCategoryRepository { get; }
        IEventPriceRepository EventPriceRepository { get; }
        ILocationRepository LocationRepository { get; }
        IOrderRepository OrderRepository { get; }
        IRoleRepository RoleRepository { get; }
        IUserRepository UserRepository { get; }
        ICityRepository CityRepository { get; }
        IOrderItemSecretRepository OrderItemSecretRepository { get; }

        void Commit();
    }
}
