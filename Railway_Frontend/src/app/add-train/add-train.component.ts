import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators, FormArray, ReactiveFormsModule } from '@angular/forms';
import { TrainService } from '../services/train-service.service';
import { TrainDto, TrainSchedule, SeatAvailability } from '../Train.Model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-add-train',
  standalone:true,
  imports:[CommonModule, ReactiveFormsModule],
  templateUrl: './add-train.component.html',
  styleUrls: ['./add-train.component.css']
})
export class AddTrainComponent implements OnInit {
  trainForm: FormGroup;

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

  ngOnInit(): void {}

  get schedules(): FormArray {
    return this.trainForm.get('Schedules') as FormArray;
  }

  get seatAvailability(): FormArray {
    return this.trainForm.get('SeatAvailability') as FormArray;
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

  

  onSubmit(): void {
    if (this.trainForm.valid) {
      const trainDto: TrainDto = this.trainForm.value;
      this.trainService.addTrain(trainDto).subscribe(response => {
        alert('Train added successfully!');
        this.trainForm.reset();
        this.schedules.clear();
        this.seatAvailability.clear();
        this.addSchedule();
        this.addSeatAvailability();
      });
    }
  }
}
