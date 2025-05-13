using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using TrainBooking.DTOs;
using TrainBooking.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TrainBooking.Models;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;
using TrainBooking.Interfaces;
using TrainBooking.Repository;

namespace TrainBooking.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    public class AuthController : ControllerBase
    {
        // private readonly UserManager<User> _userManager;


        // private readonly IConfiguration _configuration;

        private readonly IAuthRepository _authRepository;

        public AuthController(
            // UserManager<User> userManager,
            // IConfiguration configuration,
            IAuthRepository authRepository
            )
        {
            // _userManager = userManager;
            // _configuration=configuration;
            _authRepository = authRepository;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            var result = await _authRepository.Register(dto);

            // var existingUser = await _userManager.FindByNameAsync(dto.Username);
            // if (existingUser != null)
            //     return BadRequest("Username already exists");

            // var user = new User
            // {
            //     UserName = dto.Username,
            //     Email = dto.Email,
            //     AadharNumber = dto.AadharNumber 
            // };

            // if(dto.Role!="User" && dto.Role!="Admin")
            // return BadRequest("Please give correct role");

            // var CheckAdhar = new Regex(@"^\d{12}$");
            //     if (!CheckAdhar.IsMatch(user.AadharNumber))
            //         return BadRequest("Give 12 digits.");

            // var result = await _userManager.CreateAsync(user, dto.Password);

            if (result.Succeeded)
            {
                return Ok("Account created.");
                // await _userManager.AddToRoleAsync(user, dto.Role);
                // return Ok(" account created.");
            }

            return BadRequest(new
            {
                Message = "Failed to create account",
                Errors = result.Errors.Select(e => e.Description)
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            // var user = await _userManager.FindByNameAsync(dto.Username);
            // if (user != null && await _userManager.CheckPasswordAsync(user, dto.Password))
            //     {
            //         var roles = await _userManager.GetRolesAsync(user);


            //         var token = await GenerateJwtToken(user);
            //         return Ok(token);
            //     }
            //     return BadRequest("Incorrect Email/Password");

            try
            {
                var token = await _authRepository.Login(dto);
                return Ok(new{token=token});
            }
            catch (Exception ex)
            {
                return BadRequest(new{message=ex.Message});
            }
        }

        // private async Task<string> GenerateJwtToken(User user)
        // {
        //     var jwtSettings = _configuration.GetSection("JWT");
        //     var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);
        //     var claims = new List<Claim>
        //     {
        //         new Claim(JwtRegisteredClaimNames.Sub, user.Email!),
        //         new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        //         new Claim(ClaimTypes.Email, user.Email!)
        //     };
        //     var roles = await _userManager.GetRolesAsync(user);
        //     foreach (var role in roles)
        //     {
        //         claims.Add(new Claim(ClaimTypes.Role, role));
        //     }
        //     var token = new JwtSecurityToken(
        //         issuer: jwtSettings["ValidIssuer"],
        //         audience: jwtSettings["ValidAudience"],
        //         claims: claims,
        //         expires: DateTime.UtcNow.AddDays(1),
        //         signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
        //     );
        //     return new JwtSecurityTokenHandler().WriteToken(token);
        // }

        [Authorize(Roles = "Admin")]
        [HttpGet("users")]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                var users = await _authRepository.GetAllUsers();
                return Ok(users);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Error fetching users", Error = ex.Message });
            }
        }

        [Authorize(Roles = "User,Admin")]
        [HttpGet("user")]
        public IActionResult UserOrAdmin() => Ok("Hello User or Admin!");
    }
}
