using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TrainBooking.Models
{
    public class Notification
{
    [Key]
    public int NotificationID { get; set; }

    [ForeignKey("User")]
    public string Username { get; set; }

    [ForeignKey("Train")]
    public int TrainID { get; set; }

    public DateTime NotifiedOn { get; set; }
    public string Type { get; set; }
    public string Message { get; set; }

    public User User { get; set; }
    public Train Train { get; set; }
}
}