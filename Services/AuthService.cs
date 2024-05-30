using SecondTimeAttempt.Models.Domain;
using SecondTimeAttempt.Models.DTO;
using SecondTimeAttempt.Repositories;
using System.Threading.Tasks;
using BCrypt.Net;

namespace SecondTimeAttempt.Services
{
    public interface IAuthService
    {
        Task<ResponseDto> Register(UserRegisterDto request);
        Task<string> Login(UserRegisterDto request);
    }

    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly TokenService _tokenService;

        public AuthService(IAuthRepository authRepository, TokenService tokenService)
        {
            _authRepository = authRepository;
            _tokenService = tokenService;
        }

        public async Task<ResponseDto> Register(UserRegisterDto request)
        {
            if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Password))
            {
                throw new ArgumentException("Email and Password are required");
            }

            var user = await _authRepository.GetUserByEmailAsync(request.Email);
            if (user != null)
            {
                throw new ArgumentException("User with this email already exists");
            }

            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
            var newUser = new User
            {
                Email = request.Email,
                PasswordHash = passwordHash
            };

            await _authRepository.AddUserAsync(newUser);
            await _authRepository.SaveChangesAsync();

            return new ResponseDto { Email = newUser.Email };
        }

        public async Task<string> Login(UserRegisterDto request)
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

            return _tokenService.CreateToken(user);
        }
    }
}

