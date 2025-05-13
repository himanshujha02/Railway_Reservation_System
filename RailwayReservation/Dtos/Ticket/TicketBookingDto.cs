using System;
using System.Collections.Generic;

namespace TrainBooking.DTOs
{
    public class TicketBookingDto
{
    
    public int TrainID { get; set; }
    public int ClassTypeID { get; set; } // 1 =s General, 2 = Sleeper, 3 = AC
    public int SourceID { get; set; }
    public int DestinationID { get; set; }
    public DateTime TravelDate { get; set; }
    public List<PassengerBookingDto>? Passengers { get; set; }
    public string? PaymentMode { get; set; }
    public bool HasInsurance { get; set; }
     public decimal InsuranceAmount { get; set; } = 0;
}

    public class PassengerBookingDto
    {
         public string? Name { get; set; }
    public int Age { get; set; }
    public string? Gender { get; set; }
    }

   public class TicketBookingResultDto
{
    public bool Success { get; set; }
    public string? Message { get; set; }
    public string? BookBy { get; set; }
    public int TicketID { get; set; }
    public decimal TotalFare { get; set; }
    public string? TrainName { get; set; }
    public int TrainNumber { get; set; }
    public string? SourceStation { get; set; }
    public string? DestinationStation { get; set; }
    public DateTime DepartureTime { get; set; }
    public DateTime ArrivalTime { get; set; }
    public string? ClassType { get; set; }
    public string? Status { get; set; }
    public bool HasInsurance { get; set; }
    public decimal InsuranceAmount { get; set; }
    public List<PassengerDto>? Passengers { get; set; }
}

public class PassengerDtoTicketShown
{
    public string? Name { get; set; }
    public int Age { get; set; }
    public string? Gender { get; set; }
    public string? SeatNumber { get; set; }
}


}