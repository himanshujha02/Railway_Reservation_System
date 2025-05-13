using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TrainBooking.DTOs;
using TrainBooking.Models;
using TrainBooking.Data;
using TrainBooking.Interfaces;
#pragma warning restore CS8601 // Possible null reference assignment.
public class TicketPracticeRepository : ITicketRepository
{
    private readonly TrainBookingContext _context;

    public TicketPracticeRepository(TrainBookingContext context)
    {
        _context = context;
    }

    public async Task<TicketBookingResultDto> BookTicketAsync(TicketBookingDto dto, string username, string uid)
    {
        try
        {
            var sourceSchedule = await _context.TrainSchedules.FirstOrDefaultAsync(ts =>
                ts.TrainID == dto.TrainID && ts.StationID == dto.SourceID);

            var destinationSchedule = await _context.TrainSchedules.FirstOrDefaultAsync(ts =>
                ts.TrainID == dto.TrainID && ts.StationID == dto.DestinationID);

            if (sourceSchedule == null || destinationSchedule == null ||
                sourceSchedule.SequenceOrder >= destinationSchedule.SequenceOrder)
            {
                return new TicketBookingResultDto
                {
                    Success = false,
                    Message = "Invalid source or destination station."
                };
            }

            float baseFare = destinationSchedule.Fair - sourceSchedule.Fair;
            decimal multiplier = dto.ClassTypeID switch
            {
                2 => 3.0m,
                3 => 2.0m,
                _ => 1.0m
            };

            decimal farePerPassenger = (decimal)baseFare * multiplier;
            decimal totalFare = farePerPassenger * dto.Passengers.Count;
            if (dto.HasInsurance)
            {
                totalFare += dto.InsuranceAmount * dto.Passengers.Count;
            }

            var seatAvailability = await _context.SeatAvailabilities.FirstOrDefaultAsync(sa =>
                sa.TrainID == dto.TrainID &&
                sa.Date.Date == dto.TravelDate.Date &&
                sa.ClassTypeID == dto.ClassTypeID);

            int seatsNeeded = dto.Passengers.Count;

            if (seatAvailability == null || seatAvailability.RemainingSeats < seatsNeeded)
            {
                var ticket = new Ticket
                {
                    Username = uid,
                    TrainID = dto.TrainID,
                    SourceID = dto.SourceID,
                    DestinationID = dto.DestinationID,
                    JourneyDate = dto.TravelDate,
                    BookingDate = DateTime.Now,
                    ClassTypeID = dto.ClassTypeID,
                    Fare = totalFare,
                    Status = "Waiting",
                    HasInsurance = dto.HasInsurance
                };

                _context.Tickets.Add(ticket);
                await _context.SaveChangesAsync();

                foreach (var p in dto.Passengers)
                {
                    var passenger = new Passenger
                    {
                        Name = p.Name,
                        Age = p.Age,
                        Gender = p.Gender,
                        TicketID = ticket.TicketID,
                        SeatNumber = "Waiting"
                    };
                    _context.Passengers.Add(passenger);
                }

                _context.Payments.Add(new Payment
                {
                    TicketID = ticket.TicketID,
                    Amount = totalFare,
                    PaymentMode = dto.PaymentMode,
                    PaymentDate = DateTime.Now,
                    Status = "Paid",
                    IncludesInsurance = dto.HasInsurance,
                    InsuranceAmount = dto.HasInsurance ? dto.InsuranceAmount : 0
                });

                await _context.SaveChangesAsync();

                //new

                var trainForWait = await _context.Trains.FindAsync(dto.TrainID);
                var classTypeForWait = await _context.ClassTypes.FindAsync(dto.ClassTypeID);
                var sourceStationForWait = await _context.Stations.FindAsync(dto.SourceID);
                var destinationStationForWait = await _context.Stations.FindAsync(dto.DestinationID);

                var waitingPassengers = await _context.Passengers
                    .Where(p => p.TicketID == ticket.TicketID)
                    .Select(p => new PassengerDto
                    {
                        Name = p.Name,
                        Age = p.Age,
                        Gender = p.Gender,
                        SeatNumber = p.SeatNumber
                    }).ToListAsync();

#pragma warning disable CS8601 // Possible null reference assignment.
                return new TicketBookingResultDto
                {
                    BookBy = username,
                    Success = true,
                    Message = "Added to waiting list.",
                    TicketID = ticket.TicketID,
                    TotalFare = totalFare,
                    TrainName = trainForWait?.TrainName,
                    TrainNumber = dto.TrainID,
                    SourceStation = sourceStationForWait?.StationName,
                    DestinationStation = destinationStationForWait?.StationName,
                    DepartureTime = sourceSchedule.ArrivalTime,
                    ArrivalTime = destinationSchedule.ArrivalTime,
                    ClassType = classTypeForWait?.ClassName,
                    Status = ticket.Status,
                    HasInsurance = dto.HasInsurance,
                    InsuranceAmount = dto.HasInsurance ? dto.InsuranceAmount : 0,
                    Passengers = waitingPassengers
                };


            }

            seatAvailability.RemainingSeats -= seatsNeeded;

            var confirmedTicket = new Ticket
            {
                Username = uid,
                TrainID = dto.TrainID,
                SourceID = dto.SourceID,
                DestinationID = dto.DestinationID,
                JourneyDate = dto.TravelDate,
                BookingDate = DateTime.Now,
                ClassTypeID = dto.ClassTypeID,
                Fare = totalFare,
                Status = "Booked",
                HasInsurance = dto.HasInsurance
            };

            _context.Tickets.Add(confirmedTicket);
            await _context.SaveChangesAsync();

            int seatNo = 1;
            foreach (var p in dto.Passengers)
            {
                var passenger = new Passenger
                {
                    Name = p.Name,
                    Age = p.Age,
                    Gender = p.Gender,
                    TicketID = confirmedTicket.TicketID,
                    SeatNumber = $"S{seatNo++}"
                };
                _context.Passengers.Add(passenger);
            }

            _context.Payments.Add(new Payment
            {
                TicketID = confirmedTicket.TicketID,
                Amount = totalFare,
                PaymentMode = dto.PaymentMode,
                PaymentDate = DateTime.Now,
                Status = "Paid",
                IncludesInsurance = dto.HasInsurance,
                InsuranceAmount = dto.HasInsurance ? dto.InsuranceAmount : 0
            });

            await _context.SaveChangesAsync();
            //new 

            var trainForRes = await _context.Trains.FindAsync(dto.TrainID);
            var classTypeForRes = await _context.ClassTypes.FindAsync(dto.ClassTypeID);
            var sourceStationForRes = await _context.Stations.FindAsync(dto.SourceID);
            var destinationStationForRes = await _context.Stations.FindAsync(dto.DestinationID);

            var bookedPassengers = await _context.Passengers
                .Where(p => p.TicketID == confirmedTicket.TicketID)
                .Select(p => new PassengerDto
                {
                    Name = p.Name,
                    Age = p.Age,
                    Gender = p.Gender,
                    SeatNumber = p.SeatNumber
                }).ToListAsync();

            return new TicketBookingResultDto
            {
                BookBy = username,
                Success = true,
                Message = "Ticket booked successfully.",
                TicketID = confirmedTicket.TicketID,
                TotalFare = totalFare,
                TrainName = trainForRes?.TrainName,
                TrainNumber = dto.TrainID,
                SourceStation = sourceStationForRes?.StationName,
                DestinationStation = destinationStationForRes?.StationName,
                DepartureTime = sourceSchedule.DepartureTime,
                ArrivalTime = destinationSchedule.ArrivalTime,
                ClassType = classTypeForRes?.ClassName,
                Status = confirmedTicket.Status,
                HasInsurance = dto.HasInsurance,
                InsuranceAmount = dto.HasInsurance ? dto.InsuranceAmount : 0,
                Passengers = bookedPassengers
            };

        }
        catch (Exception ex)
        {
            var errorMessage = ex.InnerException?.Message ?? ex.Message;
            return new TicketBookingResultDto
            {
                Success = false,
                Message = $"Booking failed: {errorMessage}"
            };
        }
    }


    public async Task<TicketPriceResultDto> CheckTicketPriceAsync(TicketPriceCheckDto dto)
    {
        var sourceSchedule = await _context.TrainSchedules.FirstOrDefaultAsync(ts =>
            ts.TrainID == dto.TrainID && ts.StationID == dto.SourceID);

        var destinationSchedule = await _context.TrainSchedules.FirstOrDefaultAsync(ts =>
            ts.TrainID == dto.TrainID && ts.StationID == dto.DestinationID);

        if (sourceSchedule == null || destinationSchedule == null ||
            sourceSchedule.SequenceOrder >= destinationSchedule.SequenceOrder)
        {
            return new TicketPriceResultDto
            {
                Success = false,
                Message = "Invalid source or destination station."
            };
        }

        float baseFare = destinationSchedule.Fair - sourceSchedule.Fair;

        decimal multiplier = dto.ClassTypeID switch
        {
            2 => 3.0m,
            3 => 2.0m,
            _ => 1.0m
        };

        decimal farePerPassenger = (decimal)baseFare * multiplier;
        decimal totalFare = farePerPassenger * dto.PassengerCount;

        if (dto.HasInsurance)
        {
            totalFare += dto.InsuranceAmount * dto.PassengerCount;
        }

        var seatAvailability = await _context.SeatAvailabilities.FirstOrDefaultAsync(sa =>
            sa.TrainID == dto.TrainID &&
            sa.Date.Date == dto.TravelDate.Date &&
            sa.ClassTypeID == dto.ClassTypeID);

        bool seatsAvailable = seatAvailability != null && seatAvailability.RemainingSeats >= dto.PassengerCount;

        return new TicketPriceResultDto
        {
            Success = true,
            Message = seatsAvailable ? "Seats available." : "Seats not available, will be waitlisted.",
            FarePerPassenger = farePerPassenger,
            TotalFare = totalFare,
            SeatsAvailable = seatsAvailable
        };
    }

//NEW
public async Task<List<UserBookingsTicketsDto>> GetUserBookingsAsync(string username, string uid)
{
    // Load tickets with related entities efficiently
    var tickets = await _context.Tickets
        .Where(t => t.Username == uid)
        .Include(t => t.Passengers)
        .Include(t => t.Train)
        .Include(t => t.ClassType)
        .ToListAsync();

    // Get station names in one go
    var stationIds = tickets.Select(t => t.SourceID)
        .Union(tickets.Select(t => t.DestinationID))
        .Distinct()
        .ToList();

    var stations = await _context.Stations
        .Where(s => stationIds.Contains(s.StationID))
        .ToDictionaryAsync(s => s.StationID, s => s.StationName);

    // Get schedules in one go
    var trainIds = tickets.Select(t => t.TrainID).Distinct().ToList();
    var schedules = await _context.TrainSchedules
        .Where(s => trainIds.Contains(s.TrainID))
        .ToListAsync();

    // Build result
    var result = tickets.Select(ticket =>
    {
        var sourceSchedule = schedules.FirstOrDefault(s => s.TrainID == ticket.TrainID && s.StationID == ticket.SourceID);
        var destinationSchedule = schedules.FirstOrDefault(s => s.TrainID == ticket.TrainID && s.StationID == ticket.DestinationID);

        return new UserBookingsTicketsDto
        {
            TicketID = ticket.TicketID,
            TrainName = ticket.Train?.TrainName,
            TrainNumber = ticket.Train?.TrainID.ToString(),
            SourceStation = stations.GetValueOrDefault(ticket.SourceID),
            DestinationStation = stations.GetValueOrDefault(ticket.DestinationID),
            JourneyDate = ticket.JourneyDate,
            
            ClassType = ticket.ClassType?.ClassName,
            TotalFare = ticket.Fare,
            Status = ticket.Status,
            Passengers = ticket.Passengers?.Select(p => new UserBookingsPassengerDto
            {
                PassengerID = p.PassengerID,
                Name = p.Name,
                Age = p.Age,
                Gender = p.Gender,
                SeatNumber = p.SeatNumber
            }).ToList()
        };
    }).ToList();

    return result;
}

//NEW
    public async Task<DeleteResultDto> DeletePassengersAsync(DeletePassengersDto request)
{
    if (request.PassengerIds == null || request.PassengerIds.Count == 0)
    {
        return new DeleteResultDto
        {
            Success = false,
            Message = "No passengers selected for deletion.",
            RefundAmount = 0
        };
    }

    var ticket = await _context.Tickets
        .Include(t => t.Passengers)
        .FirstOrDefaultAsync(t => t.TicketID == request.TicketId);

    if (ticket == null)
    {
        return new DeleteResultDto
        {
            Success = false,
            Message = "Ticket not found.",
            RefundAmount = 0
        };
    }

    var passengersToDelete = ticket.Passengers
        .Where(p => request.PassengerIds.Contains(p.PassengerID))
        .ToList();

    if (passengersToDelete.Count == 0)
    {
        return new DeleteResultDto
        {
            Success = false,
            Message = "No matching passengers found in the ticket.",
            RefundAmount = 0
        };
    }

    var seatAvailability = await _context.SeatAvailabilities.FirstOrDefaultAsync(sa =>
        sa.TrainID == ticket.TrainID &&
        sa.Date.Date == ticket.JourneyDate.Date &&
        sa.ClassTypeID == ticket.ClassTypeID);

    decimal perPassengerFare = ticket.Fare / ticket.Passengers.Count;
    decimal refundAmount = perPassengerFare * passengersToDelete.Count * 0.8m;

    // If all passengers selected => delete ticket
    if (passengersToDelete.Count == ticket.Passengers.Count)
    {
        if (ticket.Status == "Booked" && seatAvailability != null)
        {
            seatAvailability.RemainingSeats += passengersToDelete.Count;
        }

        _context.Passengers.RemoveRange(passengersToDelete);
        _context.Tickets.Remove(ticket);
        await _context.SaveChangesAsync();

        // Check waiting list
        if (seatAvailability != null)
        {
            var waitingTickets = await _context.Tickets
                .Include(t => t.Passengers)
                .Where(t => t.TrainID == ticket.TrainID &&
                            t.ClassTypeID == ticket.ClassTypeID &&
                            t.JourneyDate.Date == ticket.JourneyDate.Date &&
                            t.Status == "Waiting")
                .OrderBy(t => t.BookingDate)
                .ToListAsync();

            foreach (var waitingTicket in waitingTickets)
            {
                if (seatAvailability.RemainingSeats >= waitingTicket.Passengers.Count)
                {
                    int seatNo = 1;
                    foreach (var p in waitingTicket.Passengers)
                    {
                        p.SeatNumber = $"S{seatNo++}";
                        seatAvailability.RemainingSeats--;
                    }
                    waitingTicket.Status = "Booked";
                    await _context.SaveChangesAsync();
                }
                else break;
            }
        }

        return new DeleteResultDto
        {
            Success = true,
            Message = $"Entire ticket deleted. Refund: ₹{refundAmount:F2} after 20% cancellation fee.",
            RefundAmount = refundAmount
        };
    }
    else
    {
        // Partial deletion
        if (ticket.Status == "Booked" && seatAvailability != null)
        {
            seatAvailability.RemainingSeats += passengersToDelete.Count;
        }

        foreach (var passenger in passengersToDelete)
        {
            ticket.Passengers.Remove(passenger);
            _context.Passengers.Remove(passenger);
        }

        // Update total fare on ticket
        ticket.Fare -= perPassengerFare * passengersToDelete.Count;

        await _context.SaveChangesAsync();

        // Update waiting list
        if (seatAvailability != null)
        {
            var waitingTickets = await _context.Tickets
                .Include(t => t.Passengers)
                .Where(t => t.TrainID == ticket.TrainID &&
                            t.ClassTypeID == ticket.ClassTypeID &&
                            t.JourneyDate.Date == ticket.JourneyDate.Date &&
                            t.Status == "Waiting")
                .OrderBy(t => t.BookingDate)
                .ToListAsync();

            foreach (var waitingTicket in waitingTickets)
            {
                if (seatAvailability.RemainingSeats >= waitingTicket.Passengers.Count)
                {
                    int seatNo = 1;
                    foreach (var p in waitingTicket.Passengers)
                    {
                        p.SeatNumber = $"S{seatNo++}";
                        seatAvailability.RemainingSeats--;
                    }
                    waitingTicket.Status = "Booked";
                    await _context.SaveChangesAsync();
                }
                else break;
            }
        }

        return new DeleteResultDto
        {
            Success = true,
            Message = $"{passengersToDelete.Count} passenger(s) deleted. Refund: ₹{refundAmount:F2} after 20% cancellation fee.",
            RefundAmount = refundAmount
        };
    }
}

   

    public async Task<TicketWithPassengersDto> GetTicketWithPassengersAsync(int ticketId)
    {
        var ticket = await _context.Tickets
            .Include(t => t.Passengers)
            .FirstOrDefaultAsync(t => t.TicketID == ticketId);

        if (ticket == null)
            return null;

        return new TicketWithPassengersDto
        {
            TicketID = ticket.TicketID,
            BookingDate = ticket.BookingDate,
            JourneyDate = ticket.JourneyDate,
            Fare = ticket.Fare,
            Status = ticket.Status,
            HasInsurance = ticket.HasInsurance,
            Passengers = ticket.Passengers.Select(p => new PassengerDto
            {
                PassengerID = p.PassengerID,
                Name = p.Name,
                Gender = p.Gender,
                Age = p.Age,
                SeatNumber = p.SeatNumber
            }).ToList()
        };
    }
}