using System;
using System.Threading.Tasks;
using GoFlex.Core.Entities;
using GoFlex.Core.Repositories.Abstractions;

namespace GoFlex.Core.Repositories
{
    public interface IUserRepository : IRepository<User, Guid>
    {
        Task<User> GetByEmailAsync(string email);
    }
}
