using TrainBooking.DTOs;
using TrainBooking.Models;
namespace TrainBooking.Interfaces{
    public interface ITicketRepository
    {
         Task<TicketBookingResultDto> BookTicketAsync(TicketBookingDto dto,string username,string uid);

        Task<TicketPriceResultDto> CheckTicketPriceAsync(TicketPriceCheckDto dto);

          Task<List<UserBookingsTicketsDto>> GetUserBookingsAsync(string username, string uid);

         Task<TicketWithPassengersDto> GetTicketWithPassengersAsync(int ticketId);

         Task<DeleteResultDto> DeletePassengersAsync(DeletePassengersDto request);
    }
}