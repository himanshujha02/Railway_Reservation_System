using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TrainBooking.Models
{
    public class Passenger
{
    [Key]
    public int PassengerID { get; set; }

    [ForeignKey("Ticket")]
    public int TicketID { get; set; }

    public string Name { get; set; }
    public string Gender { get; set; }
    public int Age { get; set; }
    public string SeatNumber { get; set; }

    public Ticket Ticket { get; set; }
}
}