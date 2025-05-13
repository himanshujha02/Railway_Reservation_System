using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TrainBooking.Models
{
    public class Ticket
{
    [Key]
    public int TicketID { get; set; }

    [ForeignKey("User")]
    public string Username { get; set; }

    [ForeignKey("Train")]
    public int TrainID { get; set; }

    public int SourceID { get; set; }

    public int DestinationID { get; set; }

    public DateTime BookingDate { get; set; }
    public DateTime JourneyDate { get; set; }

    [ForeignKey("ClassType")]
    public int ClassTypeID { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Fare cannot be negative")]
    public decimal Fare { get; set; }

    public string Status { get; set; }
    public bool HasInsurance { get; set; }

    public User User { get; set; }
    public Train Train { get; set; }
    public ClassType ClassType { get; set; }

    public ICollection<Passenger> Passengers { get; set; }
    public ICollection<Payment> Payments { get; set; }

    

}
}