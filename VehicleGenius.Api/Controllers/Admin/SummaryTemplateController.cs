using Microsoft.AspNetCore.Mvc;
using VehicleGenius.Api.Dtos;
using VehicleGenius.Api.Services.SummaryTemplates;
using VehicleGenius.Api.Services.Vehicles;

namespace VehicleGenius.Api.Controllers.Admin;

[ApiController]
[Route("admin/summary-templates")]
public class SummaryTemplateController : ControllerBase
{
  private readonly ISummaryTemplateService _summaryTemplateService;
  private readonly IVehicleService _vehicleService;

  public SummaryTemplateController(ISummaryTemplateService summaryTemplateService, IVehicleService vehicleService)
  {
    _summaryTemplateService = summaryTemplateService;
    _vehicleService = vehicleService;
  }

  [HttpGet]
  [Route("current")]
  [ProducesResponseType(typeof (SummaryTemplateDto), StatusCodes.Status200OK)]
  public async Task<IActionResult> GetCurrent(CancellationToken ct)
  {
    var answer = await _summaryTemplateService.GetForVersionAsync(1, ct);
    return Ok(answer);
  }

  [HttpPut]
  [Route("current")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<IActionResult> UpdateCurrent(
    [FromBody] SummaryTemplateDto summaryTemplateDto,
    CancellationToken ct)
  {
    await _summaryTemplateService.UpdateForVersionAsync(1, summaryTemplateDto, ct);
    return NoContent();
  }

  [HttpGet]
  [Route("example")]
  [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
  public async Task<IActionResult> GetExample(CancellationToken ct)
  {
    var vehicles = await _vehicleService.GetVehiclesAsync(ct);
    var vehicle = vehicles.FirstOrDefault();
    if (vehicle is null)
    {
      return NotFound("No vehicles");
    }

    var summary = await _vehicleService.GetVehicleSummaryAsync(vehicle.Id, ct);
    return Ok(summary);
  }

  [HttpPost]
  [Route("validate")]
  [ProducesResponseType(typeof (SummaryTemplateValidationResultDto), StatusCodes.Status200OK)]
  public async Task<IActionResult> Validate(
    [FromBody] SummaryTemplateDto summaryTemplateDto,
    CancellationToken ct)
  {
    var answer = await _summaryTemplateService.ValidateAsync(summaryTemplateDto, ct);
    return Ok(answer);
  }
}
