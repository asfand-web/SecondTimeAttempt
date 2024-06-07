using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SecondTimeAttempt.Models.Domain.Enums;

namespace SecondTimeAttempt.Models.Domain
{
    public class User : BaseModel<Guid>
    {
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public bool IsEmailConfirmed { get; set; } = false;

        public UserVerificationStatus VerificationStatus { get; set; } = UserVerificationStatus.Pending;

        public ICollection<Post> Posts { get; set; }
    }
}
