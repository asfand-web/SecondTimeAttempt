using Microsoft.EntityFrameworkCore;
using SecondTimeAttempt.Models.Domain;

namespace SecondTimeAttempt.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        } 

        public DbSet<Post> Posts { get; set; }
        public DbSet<Comment>Comments { get; set; }
        public DbSet<User>Users { get; set; }
    }
}
