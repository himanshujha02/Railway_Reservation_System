
namespace TrainBooking.DTOs{
// For adding/updating train
public class TrainDto
{
    public required string TrainName { get; set; }
    public required string TrainType { get; set; }
    public required int TotalSeats { get; set; }

    public required string RunningDays {get; set;}
    public List<TrainScheduleDto> Schedules { get; set; }
    public List<SeatAvailabilityDto> SeatAvailability { get; set; }
}

public class TrainListDto
{
    public int TrainID { get; set; }
    public string TrainName { get; set; }
    public string TrainType { get; set; }
}

public class UpdateTrainRequest
{
    public TrainDto Train { get; set; }
    public string AdminMessage { get; set; }
}

}