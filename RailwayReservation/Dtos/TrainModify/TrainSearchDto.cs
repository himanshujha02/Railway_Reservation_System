namespace TrainBooking.DTOs
{
    public class TrainSearchDto
    {
        public required string SourceStationName { get; set; }
        public required string DestinationStationName { get; set; }
        public  required DateTime TravelDate { get; set; }
        public int? ClassTypeID { get; set; }
    }
}
