export interface TrainSearchDto {
    sourceStationName: string;
    destinationStationName: string;
    travelDate: string; // Use ISO string format for Date
    classTypeID?: number | null;
  }
  
  export interface SeatAvailabilityDtoTrainSearch {
    className: string;
    availableSeats: number;
    classTypeID:number;
  }
  
  export interface TrainSearchResultDto {
    trainID: number;
    trainName: string;
    sourceStationID:number;
    sourceStationName: string;
    destinationStationID:number;
    destinationStationName: string;
    departureTime: string;
    arrivalTime: string;
    seatAvailabilities: SeatAvailabilityDtoTrainSearch[];
  }
  