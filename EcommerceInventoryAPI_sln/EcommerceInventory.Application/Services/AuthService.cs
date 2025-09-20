using EcommerceInventory.Application.DTOs.Auth;
using EcommerceInventory.Application.Interfaces;
using EcommerceInventory.Domain.Entities;
using EcommerceInventory.Infrastructure.UnitOfWork;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace EcommerceInventory.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _uow;
        private readonly IConfiguration _config;

        public AuthService(IUnitOfWork uow, IConfiguration config)
        {
            _uow = uow;
            _config = config;
        }

        public async Task<(bool Success, string? Token, string? Error)> RegisterAsync(RegisterDto dto)
        {
            var existing = await _uow.Users.GetByEmailAsync(dto.Email);
            if (existing != null) return (false, null, "Email already registered.");

            var user = new User
            {
                Email = dto.Email,
                Username = dto.Username,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password)
            };

            await _uow.Users.AddAsync(user);
            await _uow.CommitAsync();

            var token = GenerateToken(user);
            return (true, token, null);
        }

        public async Task<(bool Success, string? Token, string? Error)> LoginAsync(LoginDto dto)
        {
            var user = await _uow.Users.GetByEmailAsync(dto.Email);
            if (user == null) return (false, null, "Invalid credentials.");

            if (!BCrypt.Net.BCrypt.Verify(dto.Password, user.PasswordHash))
                return (false, null, "Invalid credentials.");

            var token = GenerateToken(user);
            return (true, token, null);
        }

        private string GenerateToken(User user)
        {
           // var jwt = _config.GetSection("Jwt");
            //var key = Encoding.UTF8.GetBytes("SuperSecretKeyReplaceThisWithEnvOrUserSecret123");
            var key = Encoding.UTF8.GetBytes(_config.GetSection("Jwt").GetValue<string>("Key"));
            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("username", user.Username)
            };

            var token = new JwtSecurityToken(
                issuer: _config.GetSection("Jwt").GetValue<string>("Issuer"),
                audience: _config.GetSection("Jwt").GetValue<string>("Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_config.GetSection("Jwt").GetValue<int>("ExpiresInMinutes")),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
