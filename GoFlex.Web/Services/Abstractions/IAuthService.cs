using System;
using System.Threading.Tasks;
using GoFlex.Core.Entities;

namespace GoFlex.Services.Abstractions
{
    public interface IAuthService
    {
        Task<User> GetUser(string search);

        Task<bool> CreateCustomer(string email, string password);

        Task<bool> CreateOrganizer(Organizer organizer, string email, string password);

        bool VerifyPassword(User user, string password);

        Task<bool> UpdatePassword(Guid userId, string oldPassword, string newPassword);
    }
}
