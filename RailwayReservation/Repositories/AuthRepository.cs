// Repository/AuthRepository.cs
using TrainBooking.DTOs;
using TrainBooking.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using TrainBooking.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace TrainBooking.Repository
{
    public class AuthRepository : IAuthRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly IConfiguration _configuration;
        private const string AdminSecretCode = "MySecretAdminCode123";
        public AuthRepository(UserManager<User> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<IdentityResult> Register(RegisterDTO dto)
        {
            var existingUser = await _userManager.FindByNameAsync(dto.Username);
            if (existingUser != null)
                return IdentityResult.Failed(new IdentityError { Description = "Username already exists" });

            if (dto.Role != "User" && dto.Role != "Admin")
                return IdentityResult.Failed(new IdentityError { Description = "Invalid role" });

            var aadharRegex = new Regex(@"^\d{12}$");
            if (!aadharRegex.IsMatch(dto.AadharNumber))
                return IdentityResult.Failed(new IdentityError { Description = "Aadhar must be 12 digits" });

             if (dto.Role == "Admin" && dto.AdminCode != AdminSecretCode)
            return IdentityResult.Failed(new IdentityError { Description = "Invalid admin code" });

            var user = new User
            {
                UserName = dto.Username,
                Email = dto.Email,
                AadharNumber = dto.AadharNumber
            };

            var result = await _userManager.CreateAsync(user, dto.Password);
            if (result.Succeeded)
                await _userManager.AddToRoleAsync(user, dto.Role);

            return result;
        }

        public async Task<string> Login(LoginDTO dto)
        {
            var user = await _userManager.FindByNameAsync(dto.Username);
            if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
                throw new Exception("Invalid credentials");

            return await GenerateJwtToken(user);
        }

        public async Task<IEnumerable<UserDTO>> GetAllUsers()
    {
        var users = await _userManager.Users.ToListAsync();
        var userList = new List<UserDTO>();

        foreach (var user in users)
        {
            var roles = await _userManager.GetRolesAsync(user);

            userList.Add(new UserDTO
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                AadharNumber = user.AadharNumber,
                Roles = roles.ToList()
            });
        }

        return userList;
    }


        private async Task<string> GenerateJwtToken(User user)
        {
            var jwtSettings = _configuration.GetSection("JWT");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Id!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Name, user.UserName!)
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = new JwtSecurityToken(
                issuer: jwtSettings["ValidIssuer"],
                audience: jwtSettings["ValidAudience"],
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
