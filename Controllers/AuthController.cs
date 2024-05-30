using Microsoft.AspNetCore.Mvc;
using SecondTimeAttempt.Models.DTO;
using SecondTimeAttempt.Services;
using System;

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

        [HttpPost("register")]
        public async Task<ActionResult> Register(UserRegisterDto request)
        {
            try
            {
                var responseDto = await _authService.Register(request);
                return Ok(new { Message = "User successfully registered", User = responseDto });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(UserRegisterDto request)
        {
            try
            {
                string token = await _authService.Login(request);
                return Ok(new { Token = token });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message });
            }
        }
    }
}
