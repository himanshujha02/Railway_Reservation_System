import { Component, OnInit } from '@angular/core';
import { BookingService } from '../services/book-ticket.service';
import { UserBookingsTicketsDto } from '../ticket-booking.model';
import { CommonModule } from '@angular/common';
import { DeletePassengerRequest } from '../ticket-booking.model';

@Component({
  selector: 'app-my-bookings',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './my-bookings.component.html',
})
export class MyBookingsComponent implements OnInit {
  bookings: UserBookingsTicketsDto[] = [];
  refundAmount: number = 0;
  selectedPassengerIds: number[] = [];
  selectedTicket: UserBookingsTicketsDto | null = null;
  showConfirmModal: boolean = false;
  alertMessage: string = '';
  alertType: string = '';

  constructor(private bookingService: BookingService) {}

  ngOnInit(): void {
    this.bookingService.getMyBookings().subscribe({
      next: (data) => {
        this.bookings = data;
      },
      error: (err) => {
        console.error('Failed to fetch bookings:', err);
      },
    });
  }

  openDeleteModal(ticket: UserBookingsTicketsDto, passengerIds: number[]): void {
    this.selectedTicket = ticket;
    this.selectedPassengerIds = passengerIds;
    this.calculateRefund();
    this.showConfirmModal = true;
  }

  calculateRefund(): void {
    if (!this.selectedTicket || this.selectedPassengerIds.length === 0) return;

    const ticket = this.selectedTicket;
    if (!ticket.passengers || ticket.passengers.length === 0) return;
    const perPassengerFare = ticket.totalFare / ticket.passengers.length;
    this.refundAmount = perPassengerFare * this.selectedPassengerIds.length * 0.8;
  }

  getPassengerIds(passengers: any[]): number[] {
    return passengers.map(p => p.passengerID);
  }
  

  confirmDelete(): void {
    if (!this.selectedTicket || this.selectedPassengerIds.length === 0) return;

    const request: DeletePassengerRequest = {
      ticketId: this.selectedTicket.ticketID,
      passengerIds: this.selectedPassengerIds
    };

    this.bookingService
      .deletePassengers(request)
      .subscribe({
        next: (response) => {
          this.alertMessage = response.message;
          this.alertType = 'success';
          
          this.showConfirmModal = false;
          this.ngOnInit();

          setTimeout(() => {
            this.alertMessage = '';
            this.alertType = '';
          }, 3000);
        },
        error: (err) => {
          console.error('Failed to delete passengers:', err);
          this.alertMessage = 'Failed to delete passengers.';
      this.alertType = 'danger';
        },
      });
  }

  cancelModal(): void {
    this.showConfirmModal = false;
    this.selectedPassengerIds = [];
    this.selectedTicket = null;
    this.refundAmount = 0;
  }
}

