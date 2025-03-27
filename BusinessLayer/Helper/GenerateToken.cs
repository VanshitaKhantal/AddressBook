using BusinessLayer.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ModelLayer.DTO;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BusinessLayer.Helper
{
    /// <summary>
    /// Helper class for generating JWT tokens.
    /// Uses HMAC SHA256 encryption to sign tokens.
    /// Reads JWT configuration (key, issuer, audience) from app settings.
    /// </summary>
    public class GenerateToken : IGenerateToken
    {
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateToken"/> class.
        /// Injects IConfiguration to access JWT settings from configuration files.
        /// </summary>
        /// <param name="configuration">Configuration service for retrieving JWT settings.</param>
        public GenerateToken(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Generates a JWT token for an authenticated user.
        /// Includes user-specific claims (Name, Email, Role).
        /// The token is signed using HMAC SHA256 and expires in 1 hour.
        /// </summary>
        /// <param name="user">User details required for generating the token.</param>
        /// <returns>A signed JWT token as a string.</returns>
        public string GenerateJwtToken(UserResponseModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(
                _configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
