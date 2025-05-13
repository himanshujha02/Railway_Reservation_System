using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrainBooking.Models
{
    public class Station
    {
         [Key]
        public int StationID { get; set; }

        [Required(ErrorMessage = "Station name is required.")]
        public string StationName { get; set; }

        [Required(ErrorMessage = "Location is required.")]
        public string Location { get; set; }
        public ICollection<TrainSchedule> TrainSchedules { get; set; }
    }
}