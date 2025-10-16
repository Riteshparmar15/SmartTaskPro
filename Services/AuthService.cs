using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Pomelo.EntityFrameworkCore.MySql.Query.Internal;
using SmartTaskPro.DTOs;
using SmartTaskPro.Models;
using SmartTaskPro.Repositories;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SmartTaskPro.Services
{
    
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepo;
        private readonly IConfiguration _config;
        private readonly PasswordHasher<User> _hasher;

        public AuthService(IUserRepository userRepo, IConfiguration _config)
        {
            _userRepo = userRepo;
            _config = _config;
            _hasher = new PasswordHasher<User>();
        }

        public async Task<AuthResultDto> LoginAsync(LoginDto dto)
        {
            var user = await _userRepo.GetByEmail(dto.Email);
            if (user == null || user.IsDeleted)
                throw new ApplicationException("Invalid credentials");

            // Compare hashed password with input password
            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);

            if (result == PasswordVerificationResult.Failed)
                throw new ApplicationException("Invalid credentials");

            // Generate JWT token for valid user
            return await GenerateTokenAsync(user);
        }

        public async Task<AuthResultDto> RegisterAsync(RegisterDto dto)
        {
            var existingUser = _userRepo.GetByEmail(dto.Email);
            if (existingUser != null)
            {
                throw new Exception("User with this email already exists.");
            }

            var user = new User
            {
                Email = dto.Email,
                FullName = dto.FullName,
                Role = Enum.TryParse<Role>(dto.Role, true,out var role) ? role : Role.User
            };

            user.PasswordHash = _hasher.HashPassword(user, dto.Password);

            await _userRepo.AddAsync(user);
            await _userRepo.SaveChangesAsync();

            return await GenerateTokenAsync(user);
        }

        private Task<AuthResultDto> GenerateTokenAsync(User user)
        {
            var jwt = _config.GetSection("Jwt");
            var secret = jwt["Secret"];
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Email),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Role, user.Role.ToString()),
            new Claim("fullName", user.FullName ?? "")
        };

            var expiryMinutes = int.Parse(jwt["AccessTokenExpirationMinutes"] ?? "120");

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryMinutes),
                signingCredentials: creds
            );

            return Task.FromResult(new AuthResultDto
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                ExpiresAt = token.ValidTo
            });
        }
    }
}
