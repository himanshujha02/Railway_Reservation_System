namespace TrainBooking.DTOs{

public class PassengerDto
{
    public int PassengerID { get; set; }
    public string Name { get; set; }
    public string Gender { get; set; }
    public int Age { get; set; }
    public string SeatNumber { get; set; }
}


    public class TicketWithPassengersDto
{
    public int TicketID { get; set; }
    public DateTime BookingDate { get; set; }
    public DateTime JourneyDate { get; set; }
    public decimal Fare { get; set; }
    public string Status { get; set; }
    public bool HasInsurance { get; set; }
    public List<PassengerDto> Passengers { get; set; }
}

public class DeletePassengersDto
{
    public int TicketId { get; set; }
    public List<int> PassengerIds { get; set; }
}

public class DeleteResultDto
{
    public bool Success { get; set; }
    public string Message { get; set; }

    public decimal RefundAmount { get; set; }
}

};