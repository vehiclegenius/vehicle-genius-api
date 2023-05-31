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
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<IActionResult> UpdateVehicle(Guid id, string username, [FromBody] VehicleDto vehicleDto)
  {
    await _vehicleService.UpdateVehicleAsync(vehicleDto);
    await _vehicleService.AssignVehicleToUserAsync(username, id);
    return NoContent();
  }
}
