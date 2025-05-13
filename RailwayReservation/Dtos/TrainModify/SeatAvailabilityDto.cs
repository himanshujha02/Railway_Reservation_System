namespace TrainBooking.DTOs{
    public class SeatAvailabilityDto
{
    
    public required DateTime Date { get; set; }
    public int ClassTypeID { get; set; } // e.g., "Sleeper", "AC"
    public required int RemainingSeats { get; set; }
}

}