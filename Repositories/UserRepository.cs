using Microsoft.EntityFrameworkCore;
using SecondTimeAttempt.Data;
using SecondTimeAttempt.Models.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SecondTimeAttempt.Repositories
{
    public interface IUserRepository : IGenericRepository<User>
    {
        
    }

    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApiDbContext context) : base(context)
        {
        }

        
    }
}
