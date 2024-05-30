using Microsoft.EntityFrameworkCore;
using SecondTimeAttempt.Data;
using SecondTimeAttempt.Models.Domain;
using System;
using System.Threading.Tasks;

namespace SecondTimeAttempt.Repositories
{
    public interface IAuthRepository
    {
        Task<User> GetUserByEmailAsync(string email);
        Task AddUserAsync(User user);
        Task SaveChangesAsync();
    }

    public class AuthRepository : IAuthRepository
    {
        private readonly ApiDbContext _dbContext;

        public AuthRepository(ApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            try
            {
                return await _dbContext.Users.FirstOrDefaultAsync(x => x.Email.Equals(email));
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while fetching the user by email", ex);
            }
        }

        public async Task AddUserAsync(User user)
        {
            try
            {
                await _dbContext.Users.AddAsync(user);
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the user", ex);
            }
        }

        public async Task SaveChangesAsync()
        {
            try
            {
                await _dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while saving changes", ex);
            }
        }
    }
}
