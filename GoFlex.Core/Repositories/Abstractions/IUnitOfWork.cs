using System;

namespace GoFlex.Core.Repositories.Abstractions
{
    public interface IUnitOfWork
    {
        IEventRepository EventRepository { get; }
        ICategoryRepository EventCategoryRepository { get; }
        ITicketRepository EventPriceRepository { get; }
        ILocationRepository LocationRepository { get; }
        IOrderRepository OrderRepository { get; }
        IRoleRepository RoleRepository { get; }
        IUserRepository UserRepository { get; }
        ICommentRepository CommentRepository { get; }
        IOrganizerRepository OrganizerRepository { get; }
        IOrderItemRepository OrderItemRepository { get; }
    }
}
