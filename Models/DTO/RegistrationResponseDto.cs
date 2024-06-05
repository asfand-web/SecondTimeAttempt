using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SecondTimeAttempt.Models.DTO
{

    public class RegistrationResponseDto
    {
        public string Email { get; set; }

        public string Message { get; set; }
    }
}
