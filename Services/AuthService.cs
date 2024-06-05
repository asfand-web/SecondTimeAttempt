using SecondTimeAttempt.Models.Domain;
using SecondTimeAttempt.Models.DTO;
using SecondTimeAttempt.Repositories;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;
using System.Security.Claims;

namespace SecondTimeAttempt.Services
{
    public interface IAuthService
    {
        Task<RegistrationResponseDto> Register(UserRegisterDto request);
        Task<LoginResponseDto> Login(UserRegisterDto request);
        Task ConfirmEmailAsync(string token);
    }

    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly TokenService _tokenService;
        private readonly IEmailService _emailService;

        public AuthService(IAuthRepository authRepository, TokenService tokenService, IEmailService emailService)
        {
            _authRepository = authRepository;
            _tokenService = tokenService;
            _emailService = emailService;
        }

        public async Task<RegistrationResponseDto> Register(UserRegisterDto request)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var newUser = new User
            {
                Email = request.Email,
                PasswordHash = passwordHash,
                VerificationStatus = UserVerificationStatus.Pending
            };

            await _authRepository.AddUserAsync(newUser);
            await _authRepository.SaveChangesAsync();

            var emailConfirmationToken = _tokenService.CreateToken(newUser, "EmailConfirmation");
            await _emailService.SendConfirmationEmailAsync(newUser.Email, emailConfirmationToken);

            return new RegistrationResponseDto { Email = newUser.Email, Message = "User successfully registered. Email sent for confirmation." };
        }

        public async Task<LoginResponseDto> Login(UserRegisterDto request)
        {
            var user = await _authRepository.GetUserByEmailAsync(request.Email);
            if (user == null)
            {
                throw new ArgumentException("No user exists with this email");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                throw new ArgumentException("Wrong password");
            }

            if (user.VerificationStatus == UserVerificationStatus.Pending)
            {
                return new LoginResponseDto { Message = "Email verification pending" };
            }

            var token = _tokenService.CreateToken(user);
            return new LoginResponseDto { Token = token, Message = "Successfully logged in" };
        }

        public async Task ConfirmEmailAsync(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var emailClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
            var tokenType = jwtToken.Claims.FirstOrDefault(c => c.Type == "TokenType")?.Value;

            if (emailClaim == null || userIdClaim == null || tokenType != "EmailConfirmation")
            {
                throw new ArgumentException("Invalid token");
            }

            var user = await _authRepository.GetUserByEmailAsync(emailClaim);
            if (user == null || user.Id.ToString() != userIdClaim)
            {
                throw new ArgumentException("Invalid token");
            }

            user.IsEmailConfirmed = true;
            user.VerificationStatus = UserVerificationStatus.Verified;
            await _authRepository.SaveChangesAsync();
        }
    }
}
