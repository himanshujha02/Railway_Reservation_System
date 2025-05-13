 public async Task<IEnumerable<TrainSearchResultDto>> SearchTrainsAsync(TrainSearchDto dto)
{
    var sourceStation = await _context.Stations.FirstOrDefaultAsync(s => s.StationName == dto.SourceStationName);
    var destStation = await _context.Stations.FirstOrDefaultAsync(s => s.StationName == dto.DestinationStationName);

    if (sourceStation == null || destStation == null || sourceStation.StationID == destStation.StationID)
    {
        return new List<TrainSearchResultDto>(); // or throw error if needed
    }

    int sourceID = sourceStation.StationID;
    int destID = destStation.StationID;

    var scheduleGroups = await _context.TrainSchedules
        .Where(s => s.StationID == sourceID || s.StationID == destID)
        .GroupBy(s => s.TrainID)
        .ToListAsync();

    var results = new List<TrainSearchResultDto>();

    foreach (var group in scheduleGroups)
    {
        var source = group.FirstOrDefault(x => x.StationID == sourceID);
        var destination = group.FirstOrDefault(x => x.StationID == destID);

        if (source == null || destination == null) continue;
        if (source.SequenceOrder >= destination.SequenceOrder) continue;

        var seat = await _context.SeatAvailabilities.FirstOrDefaultAsync(sa =>
            sa.TrainID == group.Key &&
            sa.Date.Date == dto.TravelDate.Date &&
            (!dto.ClassTypeID.HasValue || sa.ClassTypeID == dto.ClassTypeID) &&
            sa.RemainingSeats > 0);

        if (seat == null) continue;

        var train = await _context.Trains.FindAsync(group.Key);
        var classType = await _context.ClassTypes.FindAsync(seat.ClassTypeID);

        results.Add(new TrainSearchResultDto
        {
            TrainID = train.TrainID,
            TrainName = train.TrainName,
            SourceStationName = sourceStation.StationName,
            DestinationStationName = destStation.StationName,
            DepartureTime = source.DepartureTime,
            ArrivalTime = destination.ArrivalTime,
            ClassName = classType.ClassName,
            AvailableSeats = seat.RemainingSeats
        });
    }

    return results;
}