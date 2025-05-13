using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using TrainBooking.DTOs;
using TrainBooking.Helpers;
using TrainBooking.Models;
using TrainBooking.Data;
using Microsoft.EntityFrameworkCore;
using TrainBooking.Interfaces;
[Route("api/[controller]")]
[ApiController]
public class StationController : ControllerBase
{
    private readonly IStationRepository _repository;

    public StationController(IStationRepository repository)
    {
        _repository = repository;
    }

    [HttpPost]
public async Task<IActionResult> AddStation([FromBody] StationCreateDto dto)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    try
    {
        var station = new Station
        {
            StationName = dto.StationName,
            Location = dto.Location
        };

        var result = await _repository.AddStationAsync(station);
        return Ok(new { result.StationID });
    }
    catch (ArgumentNullException ex)
    {
        return BadRequest(new { message = ex.Message });
    }
    catch (Exception ex)
    {
        return StatusCode(500, new { message = "An error occurred while adding the station.", details = ex.Message });
    }
}


    [HttpGet]
    public async Task<IActionResult> GetAllStations()
    {
        var stations = await _repository.GetAllStationsAsync();
        return Ok(stations);
    }
}
