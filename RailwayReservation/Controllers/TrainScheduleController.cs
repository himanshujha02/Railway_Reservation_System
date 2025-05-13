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
// public class TrainScheduleController : ControllerBase
// {
//     private readonly TrainBookingContext _context;

//     public TrainScheduleController(TrainBookingContext context)
//     {
//         _context = context;
//     }

//     [HttpPost]
//     public async Task<IActionResult> AddSchedule([FromBody] TrainScheduleCreateDto dto)
//     {
//         if (!ModelState.IsValid)
//             return BadRequest(ModelState);

//         var schedule = new TrainSchedule
//         {
//             TrainID = dto.TrainID,
//             StationID = dto.StationID,
//             ArrivalTime = dto.ArrivalTime,
//             DepartureTime = dto.DepartureTime,
//             SequenceOrder = dto.SequenceOrder,
//             Fair = dto.Fair,
//             DistanceFromSource = dto.DistanceFromSource
//         };

//         _context.TrainSchedules.Add(schedule);
//         await _context.SaveChangesAsync();

//         return Ok(new { schedule.ScheduleID });
//     }

//     [HttpGet("{trainId}")]
//     public async Task<IActionResult> GetTrainSchedule(int trainId)
//     {
//         var schedules = await _context.TrainSchedules
//             .Where(s => s.TrainID == trainId)
//             .OrderBy(s => s.SequenceOrder)
//             .ToListAsync();

//         return Ok(schedules);
//     }
// }