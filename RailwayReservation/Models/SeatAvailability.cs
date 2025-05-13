using System;
using System.ComponentModel.DataAnnotations;

namespace TrainBooking.Models
{
    using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class SeatAvailability
{
    [Key]
    public int AvailabilityID { get; set; }

    [ForeignKey("Train")]
    public int TrainID { get; set; }

    public DateTime Date { get; set; }

    [ForeignKey("ClassType")]
    public int ClassTypeID { get; set; }

    [Range(0, int.MaxValue, ErrorMessage = "Remaining seats can't be negative")]
    public int RemainingSeats { get; set; }

    public Train Train { get; set; }
    public ClassType ClassType { get; set; }
}

}
