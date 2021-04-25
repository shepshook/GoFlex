using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using GoFlex.Core.Entities;
using GoFlex.Core.Repositories.Abstractions;
using GoFlex.Services.Abstractions;

namespace GoFlex.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AuthService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<User> GetUser(string search)
        {
            return await _unitOfWork.UserRepository.GetByEmailAsync(search);
        }

        public async Task<bool> CreateCustomer(string email, string password)
        {
            var user = GetUser(email, password, Role.Customer);

            try
            {
                await _unitOfWork.UserRepository.InsertAsync(user);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public async Task<bool> CreateOrganizer(Organizer organizer, string email, string password)
        {
            var user = GetUser(email, password, Role.Organizer);
            organizer.User = user;

            try
            {
                await _unitOfWork.OrganizerRepository.InsertAsync(organizer);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public bool VerifyPassword(User user, string password)
        {
            return user.PasswordHash == ComputeHash(password + user.PasswordSalt);
        }

        public async Task<bool> UpdatePassword(Guid userId, string oldPassword, string newPassword)
        {
            var user = await _unitOfWork.UserRepository.GetAsync(userId);
            if (!VerifyPassword(user, oldPassword))
                return false;

            var salt = GenerateSalt();
            user.PasswordHash = ComputeHash(newPassword + salt);
            user.PasswordSalt = salt;

            await _unitOfWork.UserRepository.UpdateAsync(user);
            return true;
        }

        private User GetUser(string email, string password, Role role)
        {
            var salt = GenerateSalt();
            var hash = ComputeHash(password + salt);

            var user = new User
            {
                Email = email.ToLower(),
                PasswordHash = hash,
                PasswordSalt = salt,
                Role = role
            };

            return user;
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
