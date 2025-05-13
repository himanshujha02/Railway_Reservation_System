using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace TrainBooking.Models
{
    public class User:IdentityUser
    {
    

    [StringLength(20)]
    public string AadharNumber { get; set; }

    public ICollection<Ticket> Tickets { get; set; }
    public ICollection<Notification> Notifications { get; set; }
    }
}