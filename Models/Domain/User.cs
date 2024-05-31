using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SecondTimeAttempt.Models.Domain
{
    public class User : BaseModel<Guid> // Inherit from BaseModel<Guid>
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public ICollection<Post> Posts { get; set; }
    }
}
