import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { TrainService } from '../services/train-service.service';
import { TrainSearchDto, TrainSearchResultDto } from '../train-search.model';
import { AuthService } from '../services/auth.service';
import { Router } from '@angular/router';
import { BookDataService } from '../services/book-data.service';

@Component({
  selector: 'app-train-search',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './train-search.component.html',
  styleUrls: ['./train-search.component.css']
})
export class TrainSearchComponent {
  searchModel: TrainSearchDto = {
    sourceStationName: '',
    destinationStationName: '',
    travelDate: '',
    classTypeID :null
  };
  searchAttempted = false;
  trainResults: TrainSearchResultDto[] = [];

  constructor(private trainService: TrainService, private authService:AuthService, private router:Router, private bookDataService: BookDataService) {}

  
  onSearch(): void {
    this.searchAttempted = true;
    this.trainService.searchTrains(this.searchModel).subscribe({
      next: (results) => (this.trainResults = results),
      error: (err) => console.error('Search failed', err)
    });
  }

  bookTicket( trainID: number,
    trainName: string,
    sourceStationID: number,
    destinationStationID: number,
    classTypeID: number,
    className: string,
    arrivalTime: string,
    departureTime: string): void {
    const loggedIn = this.authService.isLoggedIn();
    console.log('Is user logged in?', loggedIn);
  
    if (!loggedIn) {
      alert('Please login to book a ticket.');
      return;
    }
    console.log('Class name received:', classTypeID);
    
    
    if (!classTypeID) {
      alert('Invalid class type selected.');
      return;
    }
  
    this.bookDataService.setBookingData({
      trainID,
      trainName,
      sourceStationID,
      sourceStationName:this.searchModel.sourceStationName,
      destinationStationID,
      destinationStationName:this.searchModel.destinationStationName,
      classID: classTypeID,
      className,
      arrivalTime,
      departureTime,
      travelDate:this.searchModel.travelDate
    });
  
    this.router.navigate(['/book-ticket']);
  }
}
