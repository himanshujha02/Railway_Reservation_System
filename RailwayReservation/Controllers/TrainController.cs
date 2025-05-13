using Microsoft.AspNetCore.Mvc;
using TrainBooking.DTOs;
using Microsoft.AspNetCore.Authorization;
using TrainBooking.Interfaces;
[Route("api/[controller]")]
[ApiController]

public class TrainSearchController : ControllerBase
{
    private readonly ITrainRepository _trainRepository;

    public TrainSearchController(ITrainRepository trainRepository)
    {
        _trainRepository = trainRepository;
    }

    [HttpPost("search")]
    public async Task<IActionResult> SearchTrains([FromBody] TrainSearchDto dto)
    {
        if (dto.SourceStationName== dto.DestinationStationName)
            return BadRequest("Source and destination stations must be different.");

       


        var results = await _trainRepository.SearchTrainsAsync(dto);

        return Ok(results);
    }
     [Authorize(Roles = "Admin")]
     [HttpPost("add")]
    public async Task<IActionResult> AddTrain([FromBody] TrainDto dto)
    {
        await _trainRepository.AddTrainAsync(dto);
        return Ok(new { message = "Train added successfully." });
    }

     [Authorize(Roles = "Admin")]
     [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateTrainAsync(int id, [FromBody] UpdateTrainRequest request)
    {
        var result = await _trainRepository.UpdateTrainAsync(id, request.Train, request.AdminMessage);
        if (!result) return NotFound(new { message = "Train not found" });

        return Ok(new { message = "Train updated successfully" });
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
public async Task<IActionResult> GetAllTrains()
{
    var result = await _trainRepository.GetAllTrainsAsync();
    return Ok(result);
}

[Authorize(Roles = "Admin")]
[HttpGet("{id}")]
public async Task<IActionResult> GetTrainById(int id)
{
    var result = await _trainRepository.GetTrainByIdAsync(id);
    if (result == null) return NotFound();
    return Ok(result);
}

}
