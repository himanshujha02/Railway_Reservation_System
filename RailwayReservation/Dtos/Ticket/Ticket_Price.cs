namespace TrainBooking.DTOs{
    public class TicketPriceCheckDto
{
    public int TrainID { get; set; }
    public int SourceID { get; set; }
    public int DestinationID { get; set; }
    public int ClassTypeID { get; set; }
    public DateTime TravelDate { get; set; }
    public int PassengerCount { get; set; }
    public bool HasInsurance { get; set; }
    public decimal InsuranceAmount { get; set; } // optional
}

public class TicketPriceResultDto
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public decimal FarePerPassenger { get; set; }
    public decimal TotalFare { get; set; }
    public bool SeatsAvailable { get; set; }
}

}