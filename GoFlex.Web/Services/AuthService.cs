using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using GoFlex.Core.Entities;
using GoFlex.Core.Repositories.Abstractions;
using GoFlex.Web.Services.Abstractions;

namespace GoFlex.Web.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public User GetUser(string search)
        {
            return _unitOfWork.UserRepository.All(x => x.Email == search).SingleOrDefault();
        }

        public bool CreateUser(string email, string password, string roleName)
        {
            var salt = GenerateSalt();
            var hash = ComputeHash(password + salt);
            var role = _unitOfWork.RoleRepository.All(x =>
                string.Equals(x.Name, roleName)).First();

            var user = new User
            {
                Email = email.ToLower(),
                PasswordHash = hash,
                PasswordSalt = salt,
                RoleId = role.Id
            };

            try
            {
                _unitOfWork.UserRepository.Insert(user);
                _unitOfWork.Commit();
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public bool VerifyPassword(User user, string password)
        {
            return user.PasswordHash == ComputeHash(password + user.PasswordSalt);
        }

        public bool UpdatePassword(Guid userId, string oldPassword, string newPassword)
        {
            var user = _unitOfWork.UserRepository.Get(userId);
            if (!VerifyPassword(user, oldPassword))
                return false;

            var salt = GenerateSalt();
            user.PasswordHash = ComputeHash(newPassword + salt);
            user.PasswordSalt = salt;

            _unitOfWork.Commit();
            return true;
        }

        private string ComputeHash(string text)
        {
            var bytes = Encoding.Unicode.GetBytes(text);
            using var sha512 = SHA512.Create();
            var hashed = sha512.ComputeHash(bytes);
            return Encoding.Unicode.GetString(hashed);
        }

        private string GenerateSalt() => ComputeHash(Guid.NewGuid().ToString());
    }
}
