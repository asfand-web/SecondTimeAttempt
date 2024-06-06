using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SecondTimeAttempt.Models.Domain;
using SecondTimeAttempt.Repositories;

namespace SecondTimeAttempt.Services
{
    public interface IUserService
    {
        Task<List<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(Guid id);
        Task<User> InsertUserAsync(User user);
        Task<User> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(Guid id);
        Task<string> GetUserVerificationStatusByIdAsync(Guid id);  // New method
    }

    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _userRepository.GetAllAsync();
        }

        public async Task<User> GetUserByIdAsync(Guid id)
        {
            return await _userRepository.GetByIdAsync(id);
        }

        public async Task<User> InsertUserAsync(User user)
        {
            return await _userRepository.InsertAsync(user);
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            return await _userRepository.UpdateAsync(user);
        }

        public async Task<bool> DeleteUserAsync(Guid id)
        {
            return await _userRepository.DeleteAsync(id);
        }

        public async Task<string> GetUserVerificationStatusByIdAsync(Guid id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            return user != null ? user.VerificationStatus.ToString() : null;
        }
    }
}
