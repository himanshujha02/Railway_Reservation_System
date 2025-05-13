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
// public class TrainController : ControllerBase
// {
//     private readonly TrainBookingContext _context;

//     public TrainController(TrainBookingContext context)
//     {
//         _context = context;
//     }

//     [HttpPost]
//     public async Task<IActionResult> AddTrain([FromBody] TrainCreateDto dto)
//     {
//         if (!ModelState.IsValid)
//             return BadRequest(ModelState);

//         var train = new Train
//         {
//             TrainName = dto.TrainName,
//             TrainType = dto.TrainType,
//             TotalSeats = dto.TotalSeats,
//             RunningDays = dto.RunningDays
//         };

//         _context.Trains.Add(train);
//         await _context.SaveChangesAsync();

//         return Ok(new { train.TrainID });
//     }

    

//     [HttpGet]
//     public async Task<IActionResult> GetAllTrains()
//     {
//         var trains = await _context.Trains.ToListAsync();
//         return Ok(trains);
//     }
// }