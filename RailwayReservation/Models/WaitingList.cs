using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TrainBooking.Models
{
    public class WaitingList
{
    [Key]
    public int WaitingListID { get; set; }

    [ForeignKey("Ticket")]
    public int TicketID { get; set; }

    [ForeignKey("Train")]
    public int TrainID { get; set; }

    [ForeignKey("ClassType")]
    public int ClassTypeID { get; set; }

    public DateTime RequestDate { get; set; }
    public int Position { get; set; }

    public Ticket Ticket { get; set; }
    public Train Train { get; set; }
    public ClassType ClassType { get; set; }
}
}