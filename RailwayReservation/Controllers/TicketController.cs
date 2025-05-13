using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainBooking.DTOs;
using TrainBooking.Models;
using TrainBooking.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Railway.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Consumes("application/json")]
    [Produces("application/json")]
    [Authorize(Roles = "User")]
    public class TicketController : ControllerBase
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly UserManager<User> _userManager;

        public TicketController(ITicketRepository ticketRepository,UserManager<User> userManager)
        {
            _ticketRepository = ticketRepository;
            _userManager=userManager;
        }

        [HttpPost("book")]
    public async Task<IActionResult> BookTicket([FromBody] TicketBookingDto dto)
    {
        if (dto == null || dto.Passengers == null || dto.Passengers.Count == 0)
        {
            return BadRequest("Invalid booking request.");
        }

        var Username = User.Identity.Name;
        var user = await _userManager.GetUserAsync(User);
        string uid=user.Id;
        var result = await _ticketRepository.BookTicketAsync(dto,Username,uid);

        if (result.Success)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }

    [HttpPost("check-price")]
    public async Task<IActionResult> CheckPrice([FromBody] TicketPriceCheckDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _ticketRepository.CheckTicketPriceAsync(dto);

        if (!result.Success)
            return BadRequest(result);

        return Ok(result);
    }


    [HttpGet("my-bookings")]
public async Task<IActionResult> GetMyBookings()
{
    try{
    var Username = User.Identity.Name;
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return NotFound("User not found.");
        string uid=user.Id;
        var bookings = await _ticketRepository.GetUserBookingsAsync(Username,uid);
        return Ok(bookings);
    }
    catch(Exception ex){
         return StatusCode(500, new { message = "An unexpected error occurred.", error = ex.Message });
    }
}

     [HttpGet("{ticketId}")]
    public async Task<ActionResult<TicketWithPassengersDto>> GetTicketWithPassengers(int ticketId)
    {
        var ticket = await _ticketRepository.GetTicketWithPassengersAsync(ticketId);
        if (ticket == null)
            return NotFound();

        return Ok(ticket);
    }

       [HttpPost("delete-passengers")]
    public async Task<IActionResult> DeletePassengers([FromBody] DeletePassengersDto request)
    {
        var result = await _ticketRepository.DeletePassengersAsync(request);
        if (!result.Success)
        {
            return BadRequest(result.Message);
        }

        return Ok(result);
    }
    }
}