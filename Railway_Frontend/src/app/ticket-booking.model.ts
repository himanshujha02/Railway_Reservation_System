export interface PassengerBookingDto {
    name: string;
    age: number;
    gender: string;
  }
  
  export interface TicketBookingDto {
    trainID: number;
    classTypeID: number;
    sourceID: number;
    destinationID: number;
    travelDate: Date;
    passengers: PassengerBookingDto[];
    paymentMode: string;
    hasInsurance: boolean;
    insuranceAmount: number;
  }
  
  export interface TicketBookingResultDto {
    bookBy: string;
  success: boolean;
  message: string;
  ticketID: number;
  totalFare: number;
  trainName: string;
  trainNumber: number;
  sourceStation: string;
  destinationStation: string;
  departureTime: string; // ISO format from C#
  arrivalTime: string;
  classType: string;
  status: string;
  hasInsurance: boolean;
  insuranceAmount: number;
  passengers: PassengerResultDto[];

  }

  export interface PassengerResultDto {
    name: string;
    age: number;
    gender: string;
    seatNumber: string;
  }

  // ticket-price.dto.ts

export interface TicketPriceCheckDto {
  trainID: number;
  sourceID: number;
  destinationID: number;
  classTypeID: number;
  travelDate: Date;
  passengerCount: number;
  hasInsurance: boolean;
  insuranceAmount: number;
}

export interface TicketPriceResultDto {
  success: boolean;
  message: string;
  farePerPassenger: number;
  totalFare: number;
  seatsAvailable: boolean;
}


// user-booking.model.ts(user contains multiple booked ticket)

export interface UserBookingsPassengerDto {
  passengerID: number;
  name?: string;
  age: number;
  gender?: string;
  seatNumber?: string;
}

export interface UserBookingsTicketsDto {
  ticketID: number;
  trainName?: string;
  trainNumber?: string;
  sourceStation?: string;
  destinationStation?: string;
  journeyDate: string; 
  classType?: string;
  totalFare: number;
  status?: string;
  passengers?: UserBookingsPassengerDto[];
}

// delete-passenger-request.interface.ts
export interface DeletePassengerRequest {
  ticketId: number;
  passengerIds: number[];
}

// delete-result.interface.ts
export interface DeleteResult {
  success: boolean;
  message: string;
  refundAmount: number;
}

  