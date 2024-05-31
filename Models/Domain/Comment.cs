using System;

namespace SecondTimeAttempt.Models.Domain
{
    public class Comment : BaseModel<Guid>
    {
        public string Text { get; set; }
        public int Rating { get; set; }

        // reference navigation property
        public Guid PostId { get; set; }
        public Post Post { get; set; }
    }
}
