using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace TrainBooking.Models
{
   public class Payment
{
    [Key]
    public int PaymentID { get; set; }

    [ForeignKey("Ticket")]
    public int TicketID { get; set; }

    [Range(0, double.MaxValue, ErrorMessage = "Amount cannot be negative")]
    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }
    public string PaymentMode { get; set; }
    public string Status { get; set; }
    public bool IncludesInsurance { get; set; }
    public decimal InsuranceAmount { get; set; }

    public Ticket Ticket { get; set; }
}
}