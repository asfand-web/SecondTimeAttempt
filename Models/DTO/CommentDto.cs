using System;
using System.ComponentModel.DataAnnotations;

namespace SecondTimeAttempt.Models.DTO
{
    public class CommentDto
    {
        [Required(ErrorMessage = "Text content is required.")]
        public string Text { get; set; } = null!;

        [Required(ErrorMessage = "Rating is required.")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }

        public Guid PostId { get; set; }
    }
}
