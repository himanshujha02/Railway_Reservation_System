namespace TrainBooking.DTOs{
    public class TrainScheduleDto
{
    public required int StationID { get; set; }
    public required DateTime ArrivalTime { get; set; }
    public required DateTime DepartureTime { get; set; }
    public required int SequenceOrder { get; set; }
    public required float Fair { get; set; }
    public required int DistanceFromSource { get; set; }
}

}