using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TrainBooking.Models
{
   using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class TrainSchedule
{
    [Key]
    public int ScheduleID { get; set; }

    [ForeignKey("Train")]
    public int TrainID { get; set; }

    [ForeignKey("Station")]
    public int StationID { get; set; }

    public DateTime ArrivalTime { get; set; }
    public DateTime DepartureTime { get; set; }
    public int SequenceOrder { get; set; }
    public float Fair { get; set; }
    public int DistanceFromSource { get; set; }

    public Train Train { get; set; }
    public Station Station { get; set; }
}

}
