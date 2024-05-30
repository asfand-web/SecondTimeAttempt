using Microsoft.EntityFrameworkCore;
using SecondTimeAttempt.Data;
using SecondTimeAttempt.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecondTimeAttempt.Repositories
{
    public interface IPostRepository : IGenericRepository<Post>
    {
        Task<List<Post>> GetAllPostsByUserIdAsync(Guid userId);
    }

    public class PostRepository : GenericRepository<Post>, IPostRepository
    {
        private readonly ApiDbContext _dbContext;

        public PostRepository(ApiDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Post>> GetAllPostsByUserIdAsync(Guid userId)
        {
            return await _dbContext.Posts
                .Include(p => p.Comments)
                .Include(p => p.User)
                .Where(p => p.UserId == userId)
                .AsNoTracking()
                .ToListAsync();
        }
    }
}
