// using Microsoft.AspNetCore.Mvc;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Identity;
// using TrainBooking.DTOs;
// using TrainBooking.Helpers;
// using TrainBooking.Models;
// using TrainBooking.Data;
// using Microsoft.EntityFrameworkCore;

// [Route("api/[controller]")]
// [ApiController]
// public class SeatAvailabilityController : ControllerBase
// {
//     private readonly TrainBookingContext _context;

//     public SeatAvailabilityController(TrainBookingContext context)
//     {
//         _context = context;
//     }

//     [HttpPost]
//     public async Task<IActionResult> AddSeatAvailability([FromBody] SeatAvailabilityCreateDto dto)
//     {
//         if (!ModelState.IsValid)
//             return BadRequest(ModelState);

//         var availability = new SeatAvailability
//         {
//             TrainID = dto.TrainID,
//             Date = dto.TravelDate.Date,
//             ClassTypeID = dto.ClassTypeID,
//             RemainingSeats = dto.AvailableSeats
//         };

//         _context.SeatAvailabilities.Add(availability);
//         await _context.SaveChangesAsync();

//         return Ok(new { availability.AvailabilityID });
//     }

//     [HttpGet("{trainId}/{travelDate}")]
//     public async Task<IActionResult> GetAvailability(int trainId, DateTime travelDate)
//     {
//         var available = await _context.SeatAvailabilities
//             .Where(s => s.TrainID == trainId && s.Date == travelDate.Date)
//             .ToListAsync();

//         return Ok(available);
//     }
// }
