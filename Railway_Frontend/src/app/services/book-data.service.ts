import { Injectable } from '@angular/core';
import { TicketBookingResultDto } from '../ticket-booking.model';
@Injectable({
  providedIn: 'root'
})
export class BookDataService {
  private bookingData: any;
  private ticketResult: TicketBookingResultDto | null = null;

  setBookingData(data: any): void {
    this.bookingData = data;
  }

  getBookingData(): any {
    return this.bookingData;
  }

  clearBookingData(): void {
    this.bookingData = null;
  }

  setTicketResult(ticket: TicketBookingResultDto) {
    this.ticketResult = ticket;
  }
  
  getTicketResult(): TicketBookingResultDto | null {
    return this.ticketResult;
  }
}
