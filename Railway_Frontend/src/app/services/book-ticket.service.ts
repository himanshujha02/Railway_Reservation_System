import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { TicketBookingDto, TicketBookingResultDto, TicketPriceCheckDto, TicketPriceResultDto,UserBookingsTicketsDto, UserBookingsPassengerDto, DeletePassengerRequest, DeleteResult } from '../ticket-booking.model';
import { Observable } from 'rxjs';
import { AuthService } from './auth.service';

@Injectable({
  providedIn: 'root'
})
export class BookingService {
  private baseUrl = 'http://localhost:5206/api/Ticket';

  constructor(private http: HttpClient, private authService:AuthService) {}

  private getAuthHeaders(): HttpHeaders {
    const token = this.authService.getToken();
    return new HttpHeaders({
      'Authorization': `Bearer ${token}`
    });
  }

  bookTicket(dto: TicketBookingDto): Observable<TicketBookingResultDto> {
    
    return this.http.post<TicketBookingResultDto>(`${this.baseUrl}/book`, dto, { headers :this.getAuthHeaders()});
  }

  checkTicketPrice(dto: TicketPriceCheckDto): Observable<TicketPriceResultDto> {
    
    return this.http.post<TicketPriceResultDto>(`${this.baseUrl}/check-price`, dto,{ headers: this.getAuthHeaders() });
  }

  getMyBookings(): Observable<UserBookingsTicketsDto[]> {
    
  
    return this.http.get<UserBookingsTicketsDto[]>(`${this.baseUrl}/my-bookings`, { headers: this.getAuthHeaders() });
  }

  deletePassengers(request: DeletePassengerRequest): Observable<DeleteResult> {
    

    const dto = {
      TicketId: request.ticketId,
      PassengerIds: request.passengerIds
    };

    return this.http.post<DeleteResult>(`${this.baseUrl}/delete-passengers`, dto, { headers: this.getAuthHeaders() });
  }
}
