using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TrainBooking.Models
{
    public class ClassType
    {
        [Key]
        public int ClassTypeID { get; set; }

        [Required(ErrorMessage = "Class name is required.")]
        public string ClassName { get; set; }
        public ICollection<Ticket> Tickets { get; set; }
    public ICollection<SeatAvailability> SeatAvailabilities { get; set; }
    
    }
}