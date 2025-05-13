import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TicketBookingResultDto } from '../ticket-booking.model'; // adjust path as needed
import { BookDataService } from '../services/book-data.service'; // where you might store shared data
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-show-ticket',
  standalone:true,
  imports:[CommonModule],
  templateUrl: './show-ticket.component.html',
  styleUrls: ['./show-ticket.component.css']
})
export class ShowTicketComponent implements OnInit {
  ticket!: TicketBookingResultDto;

  constructor(private router: Router, private bookDataService: BookDataService) {}

  ngOnInit(): void {
    const data = this.bookDataService.getTicketResult();
    if (!data) {
      alert('No ticket info found!');
      this.router.navigate(['/train-search']);
      return;
    }
    this.ticket = data;
  }

  printTicket(): void {
    const printContents = document.getElementById('ticket-content')?.innerHTML;
    const originalContents = document.body.innerHTML;
  
    if (printContents) {
      document.body.innerHTML = printContents;
      window.print();
      document.body.innerHTML = originalContents;
       window.location.reload(); // optional, to reset state
    }
  }
  
}
