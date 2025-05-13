using TrainBooking.DTOs;
using Microsoft.AspNetCore.Identity;
using TrainBooking.Models;

namespace TrainBooking.Interfaces
{
    public interface IAuthRepository
    {
        Task<IdentityResult> Register(RegisterDTO dto);
        Task<string> Login(LoginDTO dto);

        Task<IEnumerable<UserDTO>> GetAllUsers();
    }
}
