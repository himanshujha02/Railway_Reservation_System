import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormArray, Validators, ReactiveFormsModule, FormsModule } from '@angular/forms';
import { TrainService } from '../services/train-service.service';
import { TrainDto, TrainListDto, UpdateTrainAdminMessage } from '../Train.Model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-update-train',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule,FormsModule],
  templateUrl: './update-train.component.html',
  styleUrls: ['./update-train.component.css']
})
export class UpdateTrainComponent implements OnInit {
  trainForm: FormGroup;
  trainList: TrainListDto[] = [];
  selectedTrainId: number = 0;
  adminMessage: string = '';


  constructor(private fb: FormBuilder, private trainService: TrainService) {
    this.trainForm = this.fb.group({
      TrainName: ['', Validators.required],
      TrainType: ['', Validators.required],
      TotalSeats: [0, [Validators.required, Validators.min(1)]],
      RunningDays: ['', Validators.required],
      Schedules: this.fb.array([]),
      SeatAvailability: this.fb.array([]),
     
    });
  }

  ngOnInit(): void {
    this.fetchTrains();
  }

  fetchTrains() {
    this.trainService.getAllTrains().subscribe((trains: TrainListDto[]) => {
      this.trainList = trains;
    });
  }

  get schedules(): FormArray {
    return this.trainForm.get('Schedules') as FormArray;
  }

  get seatAvailability(): FormArray {
    return this.trainForm.get('SeatAvailability') as FormArray;
  }

  onTrainSelect(train: TrainListDto): void {
    this.selectedTrainId = train.trainID;
    console.log(this.selectedTrainId);
    
    if (!this.selectedTrainId) return;

    this.trainService.getTrainById(this.selectedTrainId).subscribe((train: any) => {
      console.log(train);
      
      this.trainForm.patchValue({
        TrainName: train.trainName,
        TrainType: train.trainType,
        TotalSeats: train.totalSeats,
        RunningDays: train.runningDays,
      });

      // Clear previous schedules and seat availability
      this.schedules.clear();
      if (Array.isArray(train.schedules)) {
        train.schedules.forEach((schedule: { stationID: any; arrivalTime: any; departureTime: any; sequenceOrder: any; fair: any; distanceFromSource: any; }) => {
          this.schedules.push(this.fb.group({
            StationID: schedule.stationID,
            ArrivalTime: schedule.arrivalTime,
            DepartureTime: schedule.departureTime,
            SequenceOrder: schedule.sequenceOrder,
            Fair: schedule.fair,
            DistanceFromSource: schedule.distanceFromSource,
          }));
        });
      }

      this.seatAvailability.clear();
      if (Array.isArray(train.seatAvailability)) {
        train.seatAvailability.forEach((seat: { date: any; classTypeID: any; remainingSeats: any; }) => {
          const formattedDate = seat.date.split('T')[0];

          this.seatAvailability.push(this.fb.group({
            Date: formattedDate,
            ClassTypeID: seat.classTypeID,
            RemainingSeats: seat.remainingSeats,
          }));
        });
      }
    });
  }

  updateTrain() {
    if (this.trainForm.valid && this.selectedTrainId) {
      const updatedTrain: TrainDto = this.trainForm.value;
      const payload: UpdateTrainAdminMessage = {
    train: updatedTrain,
    adminMessage: this.adminMessage  };

      this.trainService.updateTrain(this.selectedTrainId, payload,).subscribe(() => {
        alert('Train updated successfully!');
      });
    }
  }

  addSchedule(): void {
    this.schedules.push(this.fb.group({
      StationID: [null, Validators.required],
      ArrivalTime: ['', Validators.required],
      DepartureTime: ['', Validators.required],
      SequenceOrder: [0, Validators.required],
      Fair: [0, Validators.required],
      DistanceFromSource: [0, Validators.required],
    }));
  }

  addSeatAvailability(): void {
    this.seatAvailability.push(this.fb.group({
      Date: ['', Validators.required],
      ClassTypeID: [null, Validators.required],
      RemainingSeats: [0, Validators.required],
    }));
  }

  removeSchedule(index: number): void {
    this.schedules.removeAt(index);
  }

  removeSeatAvailability(index: number): void {
    this.seatAvailability.removeAt(index);
  }

  trackByTrainId(index: number, train: TrainListDto): number {
    return train.trainID;
  }
}
