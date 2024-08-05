using Microsoft.AspNetCore.Mvc;
using SecondTimeAttempt.Models.DTO;
using SecondTimeAttempt.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SecondTimeAttempt.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("Register")]
        public async Task<ActionResult> Register(UserRegisterDto request)
        {
            try
            {
                var responseDto = await _authService.Register(request);
                return Ok(new { Message = responseDto.Message, Email = responseDto.Email });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(UserRegisterDto request)
        {
            try
            {
                var response = await _authService.Login(request);
                if (response.Token == null)
                {
                    return Ok(new { Message = response.Message });
                }
                return Ok(new { Token = response.Token, Message = response.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPost("Confirm-Email")]
        public async Task<ActionResult> ConfirmEmail([FromBody] string token)
        {
            try
            {
                await _authService.ConfirmEmailAsync(token);
                return Ok(new { Message = "Email successfully confirmed" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
