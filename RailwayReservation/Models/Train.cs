using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrainBooking.Models
{
    public class Train
    {
        [Key]
        public int TrainID { get; set; }

        [Required(ErrorMessage = "Train name is required.")]
        public string TrainName { get; set; }

        [Required(ErrorMessage = "Train type is required.")]
        public string TrainType { get; set; }

        [Required(ErrorMessage = "Total seats are required."), Range(0, int.MaxValue, ErrorMessage = "Total seats must be non-negative.")]
        public int TotalSeats { get; set; }

        [Required(ErrorMessage = "Running days are required.")]
        public string RunningDays { get; set; }
        public ICollection<TrainSchedule> TrainSchedules { get; set; }
    public ICollection<SeatAvailability> SeatAvailabilities { get; set; }
    public ICollection<Ticket> Tickets { get; set; }
    public ICollection<Notification> Notifications { get; set; }
    }
}