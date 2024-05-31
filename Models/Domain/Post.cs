using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecondTimeAttempt.Models.Domain
{
    public class Post : BaseModel<Guid>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public ICollection<Comment> Comments { get; set; }

        // reference of user
        public Guid? UserId { get; set; }
        public User User { get; set; }
    }
}
