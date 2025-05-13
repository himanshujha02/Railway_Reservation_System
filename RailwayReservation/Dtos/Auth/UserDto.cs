namespace TrainBooking.DTOs{
public class UserDTO
{
    public required string Id { get; set; }
    public required string Username { get; set; }
    public string Email { get; set; }
    public required string AadharNumber { get; set; }
    public List<string> Roles { get; set; }=new();
}
}