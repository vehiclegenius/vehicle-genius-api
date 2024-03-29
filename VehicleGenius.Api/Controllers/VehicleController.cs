using Microsoft.AspNetCore.Mvc;
using VehicleGenius.Api.Dtos;
using VehicleGenius.Api.Services.Vehicles;

namespace VehicleGenius.Api.Controllers;

[ApiController]
[Route("vehicles")]
public class VehicleController : ControllerBase
{
  private readonly IVehicleService _vehicleService;

  public VehicleController(IVehicleService vehicleService)
  {
    _vehicleService = vehicleService;
  }

  [HttpGet]
  [Route("")]
  [ProducesResponseType(typeof(List<VehicleDto>), StatusCodes.Status200OK)]
  public async Task<IActionResult> GetVehicles(string username, CancellationToken ct)
  {
    var result = await _vehicleService.GetVehiclesAsync(username, ct);
    return Ok(result);
  }

  [HttpGet]
  [Route("{id}")]
  [ProducesResponseType(typeof(VehicleDto), StatusCodes.Status200OK)]
  public async Task<IActionResult> GetVehicle(Guid id, string username, CancellationToken ct)
  {
    if (!await _vehicleService.VehicleExistsAsync(id, ct) ||
        !await _vehicleService.UserOwnsVehicleAsync(id, username, ct))
    {
      return NotFound();
    }

    var result = await _vehicleService.GetSingleVehicleAsync(id, ct);
    return Ok(result);
  }

  [HttpPut]
  [Route("{id}")]
  [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
  public async Task<IActionResult> UpdateVehicle(Guid id, string username, [FromBody] VehicleDto vehicleDto)
  {
    var vehicle = await _vehicleService.UpsertVehicleAsync(vehicleDto);
    await _vehicleService.AssignVehicleToUserAsync(username, vehicle.Id);
    return Ok(vehicle.Id);
  }

  [HttpPost]
  [Route("{vin}/fetch-dimo")]
  [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
  public async Task<IActionResult> FetchDimoData(string vin, string username, CancellationToken ct)
  {
    await _vehicleService.AddVehicleFromDimoAsync(vin, username, ct);
    return Ok();
  }
}
