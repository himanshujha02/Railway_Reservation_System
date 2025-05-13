using TrainBooking.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
namespace TrainBooking.Interfaces{
public interface ITrainRepository
{
    Task<IEnumerable<TrainSearchResultDto>> SearchTrainsAsync(TrainSearchDto dto);
   
   Task<int> AddTrainAsync(TrainDto dto);

   Task<bool> UpdateTrainAsync(int trainId, TrainDto dto, string adminMessage = null);

   Task<List<TrainListDto>> GetAllTrainsAsync();

   Task<TrainDto> GetTrainByIdAsync(int id);
}
}