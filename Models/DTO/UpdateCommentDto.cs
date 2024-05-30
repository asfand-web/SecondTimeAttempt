using System.ComponentModel.DataAnnotations;

namespace SecondTimeAttempt.Models.DTO
{
    public class UpdateCommentDto
    {
        [Required(ErrorMessage = "Text content is required.")]
        public string Text { get; set; } = null!;

        [Required(ErrorMessage = "Rating is required.")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5.")]
        public int Rating { get; set; }
    }
}
