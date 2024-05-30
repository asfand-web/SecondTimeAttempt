using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SecondTimeAttempt.Models.Domain
{
    public class Post
    {
        [Key]
        public Guid Id { get; set; }

        [Column("Title", TypeName = "varchar(100)")]
        [Required(ErrorMessage = "Title is required.")]
        public string Title { get; set; } 

        [Required(ErrorMessage = "Description is required.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Author is required.")]
        public string Author { get; set; }

        [Required(ErrorMessage = "CreatedDate is required.")]
        public DateTime CreatedDate { get; set; }

        [Required(ErrorMessage = "UpdatedDate is required.")]
        public DateTime UpdatedDate { get; set; }
        public ICollection<Comment> Comments { get; set; }

        // reference of user

        public Guid? UserId { get; set; }
        public User User { get; set; }
    }
}
