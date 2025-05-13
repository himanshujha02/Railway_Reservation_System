using TrainBooking.DTOs;
using TrainBooking.Models;
namespace TrainBooking.Interfaces{
   public interface IStationRepository
{
    Task<Station> AddStationAsync(Station station);
    Task<List<Station>> GetAllStationsAsync();
}
}