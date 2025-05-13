namespace TrainBooking.DTOs{

  public class SeatAvailabilityDtoTrainSearch
    {
        public required string ClassName { get; set; }
        public required int AvailableSeats { get; set; }
        public required int ClassTypeID { get; set; }  // 👈 Added
    }

    public class TrainSearchResultDto
    {
        public required int TrainID { get; set; }
        public required string TrainName { get; set; }

        public required int SourceStationID { get; set; } // 👈 Added
        public required string SourceStationName { get; set; }

        public required int DestinationStationID { get; set; } // 👈 Added
        public required string DestinationStationName { get; set; }

        public required DateTime DepartureTime { get; set; }
        public required DateTime ArrivalTime { get; set; }

        public required List<SeatAvailabilityDtoTrainSearch> SeatAvailabilities { get; set; }
    }
}