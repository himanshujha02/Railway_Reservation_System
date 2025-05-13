import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { BookDataService } from '../services/book-data.service';
import { TicketBookingDto, PassengerBookingDto, TicketPriceCheckDto, TicketPriceResultDto } from '../ticket-booking.model';
import { BookingService } from '../services/book-ticket.service'; // Create this service for API call
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-book-ticket',
  standalone: true,
  imports: [CommonModule, FormsModule], 
  templateUrl: './book-ticket.component.html',
  styleUrls: ['./book-ticket.component.css']
})
export class BookTicketComponent implements OnInit {
  ticketBookingDto: TicketBookingDto;
  bookingInfo: any;
  
  paymentMode: string = 'UPI';
  hasInsurance = false;
  insuranceAmount = 0;
  bookingMessage: string = '';
  bookingSuccess = false;
  checkedPrice: number | null = null;

  constructor(
    private bookDataService: BookDataService,
    private bookingService: BookingService,
    private router: Router
  ) {
    this.ticketBookingDto = {
      trainID: 0,
      classTypeID: 0,
      sourceID: 0,
      destinationID: 0,
      travelDate: new Date(),
      passengers: [],
      paymentMode: '',
      hasInsurance: false,
      insuranceAmount: 0
    };
  }

  ngOnInit(): void {
    const data = this.bookDataService.getBookingData();
    if (!data) {
      alert('No booking data found. Please search again.');
      this.router.navigate(['/search']);
      return;
    }

    this.bookingInfo = data;
    console.log(data);
    
    // Populate ticketBookingDto with initial data
    this.ticketBookingDto.trainID = data.trainID;
    this.ticketBookingDto.classTypeID = data.classID;
    this.ticketBookingDto.sourceID = data.sourceStationID;
    this.ticketBookingDto.destinationID = data.destinationStationID;
    this.ticketBookingDto.travelDate = data.travelDate; // Ideally get from user or pass via service
  }

  addPassenger() {
    this.ticketBookingDto.passengers.push({ name: '', age: 0, gender: '' });
  }

  removePassenger(index: number) {
    this.ticketBookingDto.passengers.splice(index, 1);
  }

  isPassengerInfoValid(): boolean {
    return this.ticketBookingDto.passengers.length > 0 &&
      this.ticketBookingDto.passengers.every(p =>
        p.name.trim() !== '' &&
        p.age > 0 &&
        p.gender.trim() !== ''
      );
  }

  checkTicketPrice(): void {

    if (!this.isPassengerInfoValid()) {
      alert('Please fill in all passenger details before checking the fare.');
      return;
    }

    const checkDto: TicketPriceCheckDto = {
      trainID: this.ticketBookingDto.trainID,
      classTypeID: this.ticketBookingDto.classTypeID,
      sourceID: this.ticketBookingDto.sourceID,
      destinationID: this.ticketBookingDto.destinationID,
      travelDate: this.ticketBookingDto.travelDate,
      passengerCount: this.ticketBookingDto.passengers.length,
      hasInsurance: this.hasInsurance,
      insuranceAmount: this.hasInsurance ? this.insuranceAmount : 0
    };
  
    this.bookingService.checkTicketPrice(checkDto).subscribe({
      next: (res: TicketPriceResultDto) => {
        this.checkedPrice = res.totalFare;
      },
      error: (err) => {
        console.error('Failed to check price', err);
        this.checkedPrice = null;
        alert('Unable to fetch price. Please try again.');
      }
    });
  }
  



  submitBooking() {

    if (!this.isPassengerInfoValid()) {
      alert('Please enter valid passenger details before booking.');
      return;
    }

    this.ticketBookingDto.paymentMode = this.paymentMode;
    this.ticketBookingDto.hasInsurance = this.hasInsurance;
    this.ticketBookingDto.insuranceAmount = this.hasInsurance ? this.insuranceAmount : 0;
    console.log(this.ticketBookingDto);
    
    this.bookingService.bookTicket(this.ticketBookingDto).subscribe({
      next: (response) => {
        this.bookingMessage = response.message;
        this.bookingSuccess = response.success;
        this.bookDataService.setTicketResult(response); // store result
  this.router.navigate(['/show-ticket']); // navigate
        
      },
      error: (err) => {
        console.error('Booking failed', err);
        this.bookingMessage = 'Booking failed. Please try again.';
        this.bookingSuccess = false;
      }
    });
  }
}
