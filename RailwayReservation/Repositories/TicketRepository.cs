// using Microsoft.EntityFrameworkCore;
// using System.Collections.Generic;
// using System.Linq;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using TrainBooking.DTOs;
// using TrainBooking.Models;
// using TrainBooking.Data;
// using TrainBooking.Interfaces;


// public class TicketRepository : ITicketRepository
// {
//     private readonly TrainBookingContext _context;
    


//     public TicketRepository(TrainBookingContext context)
//     {
//         _context = context;
        
//     }

//     public async Task<TicketBookingResultDto> BookTicketAsync(TicketBookingDto dto,string username,string uid)
// {
//     try
//     {
        
//         var sourceSchedule = await _context.TrainSchedules.FirstOrDefaultAsync(ts =>
//             ts.TrainID == dto.TrainID && ts.StationID == dto.SourceID);

//         var destinationSchedule = await _context.TrainSchedules.FirstOrDefaultAsync(ts =>
//             ts.TrainID == dto.TrainID && ts.StationID == dto.DestinationID);

//         if (sourceSchedule == null || destinationSchedule == null ||
//             sourceSchedule.SequenceOrder >= destinationSchedule.SequenceOrder)
//         {
//             return new TicketBookingResultDto
//             {
//                 Success = false,
//                 Message = "Invalid source or destination station."
//             };
//         }

//         float baseFare = destinationSchedule.Fair - sourceSchedule.Fair;

//         decimal multiplier = 1.0m;
//         if (dto.ClassTypeID == 2)
//         {
//             multiplier = 2.0m;
//         }
//         else if (dto.ClassTypeID == 3)
//         {
//             multiplier = 3.0m;
//         }

//         decimal farePerPassenger = (decimal)baseFare * multiplier;
//         decimal totalFare = farePerPassenger * dto.Passengers.Count;

//         var seatAvailability = await _context.SeatAvailabilities.FirstOrDefaultAsync(sa =>
//             sa.TrainID == dto.TrainID &&
//             sa.Date.Date == dto.TravelDate.Date &&
//             sa.ClassTypeID == dto.ClassTypeID);

//         int seatsNeeded = dto.Passengers.Count;

//         if (seatAvailability == null || seatAvailability.RemainingSeats < seatsNeeded)
//         {
//             return new TicketBookingResultDto
//             {
//                 Success = false,
//                 Message = "Insufficient seats available."
//             };
//         }

//         seatAvailability.RemainingSeats -= seatsNeeded;
//         // Create ticket
//         var ticket = new Ticket
//         {
//             Username=uid,
//             TrainID = dto.TrainID,
            
//             SourceID = dto.SourceID,
//             DestinationID = dto.DestinationID,
//             JourneyDate = dto.TravelDate,
//             BookingDate = DateTime.Now,
//             ClassTypeID=dto.ClassTypeID,
//             Fare = totalFare,
//             Status="Booked",
//             HasInsurance=dto.HasInsurance
//         };

//         _context.Tickets.Add(ticket);
//         await _context.SaveChangesAsync();

//         // Add passengers
//         int seatNo = 1;
//         foreach (var p in dto.Passengers)
//         {
//             _context.Passengers.Add(new Passenger
//             {
//                 Name = p.Name,
//                 Age = p.Age,
//                 Gender = p.Gender,
//                 TicketID = ticket.TicketID,
//                 SeatNumber=$"S{seatNo++}"
//             });
//         }

//         // Add payment
//         _context.Payments.Add(new Payment
//         {
//             TicketID = ticket.TicketID,
//             Amount = totalFare,
//             PaymentMode = dto.PaymentMode,
//             PaymentDate = DateTime.Now,
//             Status = "Paid",
//             IncludesInsurance = dto.HasInsurance,
//             InsuranceAmount = dto.HasInsurance ? dto.InsuranceAmount : 0
//         });

//         await _context.SaveChangesAsync();

//         return new TicketBookingResultDto
//         {
//             BookBy=username,
//             Success = true,
//             Message = "Ticket booked successfully",
//             TicketID = ticket.TicketID,
//             TotalFare = totalFare
//         };
//     }
//     catch (Exception ex)
//     {
//         var errorMessage = ex.InnerException?.Message ?? ex.Message;

//     return new TicketBookingResultDto
//     {
//         Success = false,
//         Message = $"Booking failed: {errorMessage}"
//     };
//     }
// }

// public async Task<TicketWithPassengersDto> GetTicketWithPassengersAsync(int ticketId)
//     {
//         var ticket = await _context.Tickets
//             .Include(t => t.Passengers)
//             .FirstOrDefaultAsync(t => t.TicketID == ticketId);

//         if (ticket == null)
//             return null;

//         return new TicketWithPassengersDto
//         {
//             TicketID = ticket.TicketID,
//             BookingDate = ticket.BookingDate,
//             JourneyDate = ticket.JourneyDate,
//             Fare = ticket.Fare,
//             Status = ticket.Status,
//             HasInsurance = ticket.HasInsurance,
//             Passengers = ticket.Passengers.Select(p => new PassengerDto
//             {
//                 PassengerID = p.PassengerID,
//                 Name = p.Name,
//                 Gender = p.Gender,
//                 Age = p.Age,
//                 SeatNumber = p.SeatNumber
//             }).ToList()
//         };
//     }

//      public async Task<DeleteResultDto> DeletePassengersAsync(DeletePassengersDto request)
//     {
//         if (request.PassengerIds == null || request.PassengerIds.Count == 0)
//     {
//         return new DeleteResultDto
//         {
//             Success = false,
//             Message = "No passengers selected for deletion."
//         };
//     }


//         var ticket = await _context.Tickets
//             .Include(t => t.Passengers)
//             .FirstOrDefaultAsync(t => t.TicketID == request.TicketId);

//         if (ticket == null)
//     {
//         return new DeleteResultDto
//         {
//             Success = false,
//             Message = "Ticket not found."
//         };
//     }

//         var passengersToDelete = ticket.Passengers
//     .Where(p => request.PassengerIds.Contains(p.PassengerID))
//     .ToList();

//         if (passengersToDelete.Count == 0)
//     {
//         return new DeleteResultDto
//         {
//             Success = false,
//             Message = "Selected passengers not found in this ticket."
//         };
//     }

//         // Update seat availability
//         var seatAvailability = await _context.SeatAvailabilities.FirstOrDefaultAsync(sa =>
//             sa.TrainID == ticket.TrainID &&
//             sa.Date.Date == ticket.JourneyDate.Date &&
//             sa.ClassTypeID == ticket.ClassTypeID);

//         if (seatAvailability != null)
//         {
//             seatAvailability.RemainingSeats += passengersToDelete.Count;
//         }

//         _context.Passengers.RemoveRange(passengersToDelete);

//         // If all passengers deleted, remove the ticket 
//         if (ticket.Passengers.Count == passengersToDelete.Count)
//         {
//             _context.Tickets.Remove(ticket);
//         }

//         await _context.SaveChangesAsync();
//         var remainingPassengerCount = ticket.Passengers.Count - passengersToDelete.Count;
//          return new DeleteResultDto
//     {
//         Success = true,
//         Message = $"Deleted {passengersToDelete.Count} passenger(s). " +
//                   $"{(remainingPassengerCount==0 ? "Ticket also removed." : "Ticket retained.")}"
//     };
//     }
    
// }
//new 
//  public async Task<DeleteResultDto> DeletePassengersAsync(DeletePassengersDto request)
//     {
//         if (request.PassengerIds == null || request.PassengerIds.Count == 0)
//         {
//             return new DeleteResultDto
//             {
//                 Success = false,
//                 Message = "No passengers selected for deletion."
//             };
//         }

//         var ticket = await _context.Tickets.Include(t => t.Passengers).FirstOrDefaultAsync(t => t.TicketID == request.TicketId);
//         if (ticket == null)
//         {
//             return new DeleteResultDto
//             {
//                 Success = false,
//                 Message = "Ticket not found."
//             };
//         }

//         if (ticket.Passengers.All(p => request.PassengerIds.Contains(p.PassengerID)))
//         {
//             var seatAvailability = await _context.SeatAvailabilities.FirstOrDefaultAsync(sa =>
//                 sa.TrainID == ticket.TrainID &&
//                 sa.Date.Date == ticket.JourneyDate.Date &&
//                 sa.ClassTypeID == ticket.ClassTypeID);

//             int releasedSeats = 0;
//             if (ticket.Status == "Booked" && seatAvailability != null)
//             {
//                 releasedSeats = ticket.Passengers.Count;
//                 seatAvailability.RemainingSeats += releasedSeats;
//             }

//             _context.Passengers.RemoveRange(ticket.Passengers);
//             _context.Tickets.Remove(ticket);
//             await _context.SaveChangesAsync();

//             if (releasedSeats > 0)
//             {
//                 var waitingTickets = await _context.Tickets
//                     .Include(t => t.Passengers)
//                     .Where(t => t.TrainID == ticket.TrainID &&
//                                 t.ClassTypeID == ticket.ClassTypeID &&
//                                 t.JourneyDate.Date == ticket.JourneyDate.Date &&
//                                 t.Status == "Waiting")
//                     .OrderBy(t => t.BookingDate)
//                     .ToListAsync();

//                 foreach (var waitingTicket in waitingTickets)
//                 {
//                     if (seatAvailability.RemainingSeats >= waitingTicket.Passengers.Count)
//                     {
//                         int newSeatNo = 1;
//                         foreach (var p in waitingTicket.Passengers)
//                         {
//                             p.SeatNumber = $"S{newSeatNo++}";
//                             seatAvailability.RemainingSeats--;
//                         }

//                         waitingTicket.Status = "Booked";
//                         await _context.SaveChangesAsync();
//                     }
//                     else
//                     {
//                         break;
//                     }
//                 }
//             }

//             return new DeleteResultDto
//             {
//                 Success = true,
//                 Message = "Entire ticket and its passengers deleted successfully."
//             };
//         }
//         else
//         {
//             return new DeleteResultDto
//             {
//                 Success = false,
//                 Message = "Either select all passengers or none to delete the ticket. Partial deletion not allowed."
//             };
//         }
//     }