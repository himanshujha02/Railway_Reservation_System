import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Station } from '../Station.model';
import { StationService } from '../services/station.service';

@Component({
  selector: 'app-station',
  imports: [CommonModule, FormsModule],
  templateUrl: './station.component.html',
  styleUrl: './station.component.css'
})
export class StationComponent {
  stations: Station[] = [];
  newStation: Station = { stationName: '', location: '' };
  successMessage: string = '';

  constructor(private stationService: StationService) {}

  ngOnInit(): void {
    this.fetchStations();
  }

  fetchStations(): void {
    this.stationService.getAllStations().subscribe(data => {
      this.stations = data;
    });
  }

  addStation(): void {
    if (this.newStation.stationName && this.newStation.location) {
      this.stationService.addStation(this.newStation).subscribe(() => {
        this.fetchStations(); // refresh list
        this.newStation = { stationName: '', location: '' }; // reset form
        this.successMessage = 'Station created successfully';
        setTimeout(() => {
          this.successMessage = '';
        }, 3000);
      });
    }
  }
}
