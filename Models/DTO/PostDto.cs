using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using SecondTimeAttempt.Models.Domain;
using System.Collections.ObjectModel;

namespace SecondTimeAttempt.Models.DTO
{
    public class PostDto
    {

        [Column("Title", TypeName = "varchar(100)")]
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Author is required.")]
        public string Author { get; set; }

        public ICollection<CommentDto> Comments { get; set; }
        // reference navigation property
        public UserDto User { get; set; }

    }
}
