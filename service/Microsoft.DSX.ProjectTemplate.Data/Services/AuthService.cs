using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.DSX.ProjectTemplate.Data.DTOs;
using Microsoft.DSX.ProjectTemplate.Data.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Microsoft.DSX.ProjectTemplate.Data.Services
{
    public class AuthService
    {
        private readonly ProjectTemplateDbContext _dbContext;
        private readonly IConfiguration _configuration;
        
        public AuthService(ProjectTemplateDbContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }
        public string GenerateJwtToken(UserMessages user)
        {
            var jwtSettings = _configuration.GetSection("Jwt").Get<JwtSettings>();
            var key = Encoding.ASCII.GetBytes(jwtSettings.Key);
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.Username)
                    // Add other claims as needed
                }),
                Expires = DateTime.UtcNow.AddMinutes(jwtSettings.DurationInMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        
        public string Login(LoginDto loginDto)
        {
            var user = _dbContext.UserMessages.SingleOrDefault(u => u.Username == loginDto.Username);

            if (user != null && BCrypt.Net.BCrypt.Verify(loginDto.Password, user.PasswordHash))
            {
                // User credentials are correct, generate JWT token
                return GenerateJwtToken(user);
            }

            return null; // Or handle this case as you see fit
        }
    }
}