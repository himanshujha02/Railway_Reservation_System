using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrainBooking.DTOs;
using TrainBooking.Models;
using TrainBooking.Data;
using TrainBooking.Interfaces;
public class TrainRepository : ITrainRepository
{
    private readonly TrainBookingContext _context;

    public TrainRepository(TrainBookingContext context)
    {
        _context = context;
    }

   public async Task<IEnumerable<TrainSearchResultDto>> SearchTrainsAsync(TrainSearchDto dto)
{
    var sourceStation = await _context.Stations.FirstOrDefaultAsync(s => s.StationName == dto.SourceStationName);
    var destStation = await _context.Stations.FirstOrDefaultAsync(s => s.StationName == dto.DestinationStationName);

    if (sourceStation == null || destStation == null || sourceStation.StationID == destStation.StationID)
    {
        return new List<TrainSearchResultDto>();
    }

    int sourceID = sourceStation.StationID;
    int destID = destStation.StationID;

    var scheduleGroups = await _context.TrainSchedules
        .Where(s => s.StationID == sourceID || s.StationID == destID)
        .GroupBy(s => s.TrainID)
        .ToListAsync();

    var results = new List<TrainSearchResultDto>();

    foreach (var group in scheduleGroups)
    {
        var source = group.FirstOrDefault(x => x.StationID == sourceID);
        var destination = group.FirstOrDefault(x => x.StationID == destID);

        if (source == null || destination == null) continue;
        if (source.SequenceOrder >= destination.SequenceOrder) continue;

        var seatAvailabilities = await _context.SeatAvailabilities
            .Where(sa =>
                sa.TrainID == group.Key &&
                sa.Date.Date == dto.TravelDate.Date &&
                (!dto.ClassTypeID.HasValue || sa.ClassTypeID == dto.ClassTypeID) &&
                sa.RemainingSeats > 0)
            .ToListAsync();

        if (!seatAvailabilities.Any()) continue;

        var train = await _context.Trains.FindAsync(group.Key);
        if (train == null) continue;

        var seatAvailabilityDtos = new List<SeatAvailabilityDtoTrainSearch>();

        foreach (var seat in seatAvailabilities)
        {
            var classType = await _context.ClassTypes.FindAsync(seat.ClassTypeID);
            if (classType == null) continue;

            seatAvailabilityDtos.Add(new SeatAvailabilityDtoTrainSearch
            {
                ClassName = classType.ClassName,
                ClassTypeID = seat.ClassTypeID,
                AvailableSeats = seat.RemainingSeats
            });
        }

        results.Add(new TrainSearchResultDto
        {
            TrainID = train.TrainID,
            TrainName = train.TrainName,
            SourceStationID = sourceStation.StationID,
            SourceStationName = sourceStation.StationName,
            DestinationStationID = destStation.StationID,
            DestinationStationName = destStation.StationName,
            DepartureTime = source.DepartureTime,
            ArrivalTime = destination.ArrivalTime,
            SeatAvailabilities = seatAvailabilityDtos
        });
    }

    return results;
}




public async Task<int> AddTrainAsync(TrainDto dto)
{
    var train = new Train
    {
        TrainName = dto.TrainName,
        TrainType = dto.TrainType,
        TotalSeats = dto.TotalSeats,
        RunningDays = dto.RunningDays
    };

    _context.Trains.Add(train);
    await _context.SaveChangesAsync(); // get TrainID

    foreach (var scheduleDto in dto.Schedules)
    {
        _context.TrainSchedules.Add(new TrainSchedule
        {
            TrainID = train.TrainID,
            StationID = scheduleDto.StationID,
            ArrivalTime = scheduleDto.ArrivalTime,
            DepartureTime = scheduleDto.DepartureTime,
            SequenceOrder = scheduleDto.SequenceOrder,
            Fair = scheduleDto.Fair,
            DistanceFromSource = scheduleDto.DistanceFromSource
        });
    }

    foreach (var seatDto in dto.SeatAvailability)
    {
        _context.SeatAvailabilities.Add(new SeatAvailability
        {
            TrainID = train.TrainID,
            Date = seatDto.Date,
            ClassTypeID = seatDto.ClassTypeID,
            RemainingSeats = seatDto.RemainingSeats
        });
    }

    await _context.SaveChangesAsync();
    return train.TrainID;
}

public async Task<bool> UpdateTrainAsync(int trainId, TrainDto dto, string adminMessage = null)
{
    var train = await _context.Trains.FindAsync(trainId);
    if (train == null) return false;

    // Update train properties
    train.TrainName = dto.TrainName;
    train.TrainType = dto.TrainType;
    train.TotalSeats = dto.TotalSeats;
    train.RunningDays = dto.RunningDays;

    // Remove existing schedules
    var existingSchedules = _context.TrainSchedules.Where(s => s.TrainID == trainId);
    _context.TrainSchedules.RemoveRange(existingSchedules);

    // Add new schedules
    foreach (var scheduleDto in dto.Schedules)
    {
        _context.TrainSchedules.Add(new TrainSchedule
        {
            TrainID = trainId,
            StationID = scheduleDto.StationID,
            ArrivalTime = scheduleDto.ArrivalTime,
            DepartureTime = scheduleDto.DepartureTime,
            SequenceOrder = scheduleDto.SequenceOrder,
            Fair = scheduleDto.Fair,
            DistanceFromSource = scheduleDto.DistanceFromSource
        });
    }

    // Remove existing seat availability
    var existingSeats = _context.SeatAvailabilities.Where(s => s.TrainID == trainId);
    _context.SeatAvailabilities.RemoveRange(existingSeats);

    // Add new seat availability
    foreach (var seatDto in dto.SeatAvailability)
    {
        _context.SeatAvailabilities.Add(new SeatAvailability
        {
            TrainID = trainId,
            Date = seatDto.Date,
            ClassTypeID = seatDto.ClassTypeID,
            RemainingSeats = seatDto.RemainingSeats
        });
    }
    Console.WriteLine(adminMessage);
    // Add notification if adminMessage is provided
    if (!string.IsNullOrWhiteSpace(adminMessage))
    {
        var affectedUsers = _context.Tickets
            .Where(t => t.TrainID == trainId)
            .Select(t => t.Username)
            .Distinct()
            .ToList();

        foreach (var username in affectedUsers)
        {
            var notification = new Notification
            {
                Username = username,
                TrainID = trainId,
                NotifiedOn = DateTime.UtcNow,
                Type = "TrainUpdate",
                Message = adminMessage
            };
            _context.Notifications.Add(notification);
        }
    }

    await _context.SaveChangesAsync();
    return true;
}

public async Task<List<TrainListDto>> GetAllTrainsAsync()
{
    return await _context.Trains
        .Select(t => new TrainListDto
        {
            TrainID = t.TrainID,
            TrainName = t.TrainName,
            TrainType = t.TrainType
        })
        .ToListAsync();
}

public async Task<TrainDto> GetTrainByIdAsync(int id)
{
    var train = await _context.Trains
        .Include(t => t.TrainSchedules)
        .Include(t => t.SeatAvailabilities)
        .FirstOrDefaultAsync(t => t.TrainID == id);

    if (train == null) return null;

    return new TrainDto
    {
        TrainName = train.TrainName,
        TrainType = train.TrainType,
        TotalSeats = train.TotalSeats,
        RunningDays = train.RunningDays,
        Schedules = train.TrainSchedules.Select(s => new TrainScheduleDto
        {
            StationID = s.StationID,
            ArrivalTime = s.ArrivalTime,
            DepartureTime = s.DepartureTime,
            SequenceOrder = s.SequenceOrder,
            Fair = s.Fair,
            DistanceFromSource = s.DistanceFromSource
        }).ToList(),
        SeatAvailability = train.SeatAvailabilities.Select(s => new SeatAvailabilityDto
        {
            Date = s.Date,
            ClassTypeID = s.ClassTypeID,
            RemainingSeats = s.RemainingSeats
        }).ToList()
    };
}


}
