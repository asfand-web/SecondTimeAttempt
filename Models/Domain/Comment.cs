using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SecondTimeAttempt.Models.Domain
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }

        public string Text { get; set; } = null!;
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // reference navigation property
        public Guid PostId { get; set; }
        public Post Post { get; set; }
    }
}
