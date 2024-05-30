using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SecondTimeAttempt.Models.DTO
{
    public class InsertPostDto
    {

        [Column("Title", TypeName = "varchar(100)")]
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Author is required.")]
        public string Author { get; set; }

        // reference navigation property
       // public Guid UserId { get; set; }

    }
}
