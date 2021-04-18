using GoFlex.Core.Repositories;
using GoFlex.Core.Repositories.Abstractions;
using GoFlex.Infrastructure.Repositories;
using Serilog;

namespace GoFlex.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private IEventRepository _eventRepository;
        private ICategoryRepository _eventCategoryRepository;
        private ITicketRepository _eventPriceRepository;
        private ILocationRepository _locationRepository;
        private IOrderRepository _orderRepository;
        private IRoleRepository _roleRepository;
        private IUserRepository _userRepository;
        private ICommentRepository _commentRepository;
        private IOrderItemRepository _orderItemRepository;
        private IOrganizerRepository _organizerRepository;

        private readonly Database _database;

        private ILogger Logger { get; }

        public IEventRepository EventRepository => _eventRepository ??= new EventRepository(_database, this);
        public ICategoryRepository EventCategoryRepository => _eventCategoryRepository ??= new CategoryRepository(_database, this);
        public ITicketRepository EventPriceRepository => _eventPriceRepository ??= new TicketRepository(_database, this);
        public ILocationRepository LocationRepository => _locationRepository ??= new LocationRepository(_database, this);
        public IOrderRepository OrderRepository => _orderRepository ??= new OrderRepository(_database, this);
        public IRoleRepository RoleRepository => _roleRepository ??= new RoleRepository(_database, this);
        public IUserRepository UserRepository => _userRepository ??= new UserRepository(_database, this);
        public ICommentRepository CommentRepository => _commentRepository ??= new CommentRepository(_database, this);
        public IOrganizerRepository OrganizerRepository => _organizerRepository ??= new OrganizerRepository(_database, this);
        public IOrderItemRepository OrderItemRepository => _orderItemRepository ??= new OrderItemRepository(_database, this);
        
        public UnitOfWork(Database database, ILogger logger)
        {
            Logger = logger.ForContext<UnitOfWork>();
            _database = database;
            Logger.Debug("Database connection established");
        }
    }
}
