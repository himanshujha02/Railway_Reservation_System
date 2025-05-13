using System.ComponentModel.DataAnnotations;

namespace TrainBooking.DTOs
{
    public class StationCreateDto
{
 [Required(ErrorMessage = "Station name is required.")]
    public required string StationName { get; set; }
[Required(ErrorMessage = "Location is required.")]
    public required string Location { get; set; }
}
}