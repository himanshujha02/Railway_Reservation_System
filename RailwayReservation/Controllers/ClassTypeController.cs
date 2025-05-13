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

// public class ClassTypeController : ControllerBase
// {
//     private readonly TrainBookingContext _context;

//     public ClassTypeController(TrainBookingContext context)
//     {
//         _context = context;
//     }

//     // POST: api/ClassType
//     [HttpPost]
//     public async Task<IActionResult> AddClassType([FromBody] ClassTypeDto dto)
//     {
//         if (!ModelState.IsValid)
//             return BadRequest(ModelState);

//         var classType = new ClassType
//         {
//             ClassName = dto.ClassName
//         };

//         _context.ClassTypes.Add(classType);
//         await _context.SaveChangesAsync();

//         return Ok(new { classType.ClassTypeID });
//     }

//     // GET: api/ClassType
//     [HttpGet]
//     public async Task<ActionResult<IEnumerable<ClassTypeDto>>> GetAllClassTypes()
//     {
//         var types = await _context.ClassTypes
//             .Select(c => new ClassTypeDto
//             {
//                 ClassTypeID = c.ClassTypeID,
//                 ClassName = c.ClassName
//             })
//             .ToListAsync();

//         return Ok(types);
//     }
// }
