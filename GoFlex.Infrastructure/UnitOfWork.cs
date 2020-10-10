using System;
using GoFlex.Core.Repositories;
using GoFlex.Core.Repositories.Abstractions;
using GoFlex.Infrastructure.Repositories;

namespace GoFlex.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private IEventRepository _eventRepository;
        private IEventCategoryRepository _eventCategoryRepository;
        private IEventPriceRepository _eventPriceRepository;
        private ILocationRepository _locationRepository;
        private IOrderRepository _orderRepository;
        private IRoleRepository _roleRepository;
        private IUserRepository _userRepository;
        private ICityRepository _cityRepository;

        private bool _isDisposed;

        private GoFlexContext Context { get; }

        public IEventRepository EventRepository => _eventRepository ??= new EventRepository(Context);
        public IEventCategoryRepository EventCategoryRepository => _eventCategoryRepository ??= new EventCategoryRepository(Context); 
        public IEventPriceRepository EventPriceRepository => _eventPriceRepository ??= new EventPriceRepository(Context);
        public ILocationRepository LocationRepository => _locationRepository ??= new LocationRepository(Context);
        public IOrderRepository OrderRepository => _orderRepository ??= new OrderRepository(Context);
        public IRoleRepository RoleRepository => _roleRepository ??= new RoleRepository(Context);
        public IUserRepository UserRepository => _userRepository ??= new UserRepository(Context);
        public ICityRepository CityRepository => _cityRepository ??= new CityRepository(Context);

        public UnitOfWork()
        {
            Context = new GoFlexContext("Server=localhost; Database=GoFlex; Integrated Security=true");
        }

        public void Dispose()
        {
            if (Context == null)
                return;

            if (!_isDisposed)
                Context.Dispose();

            _isDisposed = true;
        }
        
        public void Commit()
        {
            if (_isDisposed) 
                throw new ObjectDisposedException("UnitOfWork");

            Context.SaveChanges();
        }
    }
}
