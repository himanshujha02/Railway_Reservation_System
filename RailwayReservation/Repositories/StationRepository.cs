// StationRepository.cs
using Microsoft.EntityFrameworkCore;
using TrainBooking.Data;
using TrainBooking.Models;
using TrainBooking.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

public class StationRepository : IStationRepository
{
    private readonly TrainBookingContext _context;

    public StationRepository(TrainBookingContext context)
    {
        _context = context;
    }

    public async Task<Station> AddStationAsync(Station station)
{
    if (station == null)
        throw new ArgumentNullException(nameof(station));

    _context.Stations.Add(station);
    await _context.SaveChangesAsync();
    return station;
}


    public async Task<List<Station>> GetAllStationsAsync()
    {
        return await _context.Stations.ToListAsync();
    }
}
