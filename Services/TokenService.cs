using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SecondTimeAttempt.Models.Domain;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SecondTimeAttempt.Services
{
    public class TokenService
    {
        private readonly IConfiguration _configuration;

        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // New parameter tokenType 
        public string CreateToken(User user, string tokenType = "Access")
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Name),
                new Claim("UserID", user.Id.ToString()),
                // New claim for TokenType
                new Claim("TokenType", tokenType)
            };

            var secretKey = _configuration.GetSection("Jwt:SecretKey").Value;
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var issuer = _configuration.GetSection("Jwt:Issuer").Value;
            var audience = _configuration.GetSection("Jwt:Audience").Value;

            // New conditional expiration logic
            var expiration = tokenType is "EmailConfirmation" ? DateTime.Now.AddHours(1) : DateTime.Now.AddDays(1);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
           // Updated to use the new expiration variable
                expires: expiration,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
