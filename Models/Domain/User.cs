using System.ComponentModel.DataAnnotations;

namespace SecondTimeAttempt.Models.Domain
{
    public class User
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; } = string.Empty;    

        public string Email { get; set; }

        public string PasswordHash { get; set; }

      

        public ICollection<Post> Posts { get; set; }
    }
}
