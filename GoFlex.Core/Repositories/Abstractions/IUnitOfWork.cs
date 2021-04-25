namespace GoFlex.Core.Repositories.Abstractions
{
    public interface IUnitOfWork
    {
        IEventRepository EventRepository { get; }
        ICategoryRepository CategoryRepository { get; }
        ITicketRepository TicketRepository { get; }
        ILocationRepository LocationRepository { get; }
        IOrderRepository OrderRepository { get; }
        IUserRepository UserRepository { get; }
        ICommentRepository CommentRepository { get; }
        IOrganizerRepository OrganizerRepository { get; }
        IOrderItemRepository OrderItemRepository { get; }
    }
}
