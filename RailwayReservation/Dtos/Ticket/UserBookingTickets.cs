namespace TrainBooking.DTOs{
public class UserBookingsPassengerDto
{
    public int PassengerID { get; set; }
    public string? Name { get; set; }
    public int Age { get; set; }
    public string? Gender { get; set; }
    public string? SeatNumber { get; set; }
}
public class UserBookingsTicketsDto
{
    public int TicketID { get; set; }
    public string? TrainName { get; set; }
    public string? TrainNumber { get; set; }
    public string? SourceStation { get; set; }
    public string? DestinationStation { get; set; }
    public DateTime JourneyDate { get; set; }
    
    
    public string? ClassType { get; set; }
    public decimal TotalFare { get; set; }
    public string? Status { get; set; }
    
    public List<UserBookingsPassengerDto>? Passengers { get; set; }
}
}