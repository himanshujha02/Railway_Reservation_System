export interface TrainSchedule {
    StationID: number;
    ArrivalTime: Date;
    DepartureTime: Date;
    SequenceOrder: number;
    Fair: number;
    DistanceFromSource: number;
  }
  
  export interface SeatAvailability {
    Date: string;
    ClassTypeID: number;
    RemainingSeats: number;
  }
  
  export interface TrainDto {
    TrainName: string;
    TrainType: string;
    TotalSeats: number;
    RunningDays: string;
    Schedules: TrainSchedule[];
    SeatAvailability: SeatAvailability[];
  }

  export interface TrainListDto {
    trainID: number;
    trainName: string;
    trainType: string;
  }

  export interface UpdateTrainAdminMessage {
  train: TrainDto;
  adminMessage: string;
}

  
  