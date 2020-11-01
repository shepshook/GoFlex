using System;
using GoFlex.Core.Entities;

namespace GoFlex.Web.Services.Abstractions
{
    public interface IAuthService
    {
        User GetUser(string search);

        bool CreateUser(string email, string password, string roleName);

        bool VerifyPassword(User user, string password);

        bool UpdatePassword(Guid userId, string oldPassword, string newPassword);
    }
}
