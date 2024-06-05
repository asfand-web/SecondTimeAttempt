using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SecondTimeAttempt.Models.DTO
{
    public class UserDto
    {
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid Email format")]
        public string Email { get; set; }
        public bool IsEmailConfirmed { get; set; } = false;

        public required string PasswordHash { get; set; }
    }
}
