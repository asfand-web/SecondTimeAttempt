using Microsoft.EntityFrameworkCore;
using SecondTimeAttempt.Data;
using SecondTimeAttempt.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SecondTimeAttempt.Repositories
{
    public interface ICommentRepository : IGenericRepository<Comment>
    {
        Task<List<Comment>> GetAllCommentsByUserIdAsync(Guid userId);
    }

    public class CommentRepository : GenericRepository<Comment>, ICommentRepository
    {
        private readonly ApiDbContext _dbContext;

        public CommentRepository(ApiDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Comment>> GetAllCommentsByUserIdAsync(Guid userId)
        {
            return await _dbContext.Comments
                .Include(c => c.Post)
                .Where(c => c.Post.UserId == userId)
                .ToListAsync();
        }
    }
}
