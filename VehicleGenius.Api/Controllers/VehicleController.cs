using Microsoft.AspNetCore.Mvc;
using VehicleGenius.Api.Dtos;
using VehicleGenius.Api.Services;

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
  [Produces(typeof(List<VehicleDto>))]
  public async Task<IActionResult> GetVehicles(CancellationToken ct)
  {
    var result = await _vehicleService.GetVehiclesAsync(ct);
    return Ok(result);
  }

  [HttpGet]
  [Route("{id}")]
  [Produces(typeof(VehicleDto))]
  public async Task<IActionResult> GetVehicle(Guid id, CancellationToken ct)
  {
    if (!await _vehicleService.VehicleExistsAsync(id, ct))
    {
      return NotFound();
    }

    var result = await _vehicleService.GetSingleVehicleAsync(id, ct);
    return Ok(result);
  }

  [HttpPut]
  [Route("{id}")]
  public async Task<IActionResult> UpdateVehicle(Guid id, [FromBody] VehicleDto vehicleDto)
  {
    await _vehicleService.UpdateVehicleAsync(vehicleDto);
    return NoContent();
  }
}
