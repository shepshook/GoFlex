using GoFlex.Core.Repositories;
using GoFlex.Core.Repositories.Abstractions;
using GoFlex.Infrastructure.Repositories;

namespace GoFlex.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private IEventRepository _eventRepository;
        private ICategoryRepository _categoryRepository;
        private ITicketRepository _ticketRepository;
        private ILocationRepository _locationRepository;
        private IOrderRepository _orderRepository;
        private IUserRepository _userRepository;
        private ICommentRepository _commentRepository;
        private IOrderItemRepository _orderItemRepository;
        private IOrganizerRepository _organizerRepository;

        private readonly Database _database;

        public IEventRepository EventRepository => _eventRepository ??= new EventRepository(_database, this);
        public ICategoryRepository CategoryRepository => _categoryRepository ??= new CategoryRepository(_database, this);
        public ITicketRepository TicketRepository => _ticketRepository ??= new TicketRepository(_database, this);
        public ILocationRepository LocationRepository => _locationRepository ??= new LocationRepository(_database, this);
        public IOrderRepository OrderRepository => _orderRepository ??= new OrderRepository(_database, this);
        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_database, this);

        public ICommentRepository CommentRepository => _commentRepository ??= new CommentRepository(_database, this);
        public IOrganizerRepository OrganizerRepository => _organizerRepository ??= new OrganizerRepository(_database, this);
        public IOrderItemRepository OrderItemRepository => _orderItemRepository ??= new OrderItemRepository(_database, this);

        public UnitOfWork(Database database)
        {
            _database = database;
        }
    }
}
