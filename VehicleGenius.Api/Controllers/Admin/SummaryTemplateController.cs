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
  public async Task<ActionResult<SummaryTemplateDto>> GetCurrent(CancellationToken ct)
  {
    var answer = await _summaryTemplateService.GetForVersionAsync(1, ct);
    return Ok(answer);
  }

  [HttpPut]
  [Route("current")]
  public async Task<ActionResult<SummaryTemplateDto>> UpdateCurrent(
    [FromBody] SummaryTemplateDto summaryTemplateDto,
    CancellationToken ct)
  {
    await _summaryTemplateService.UpdateForVersionAsync(1, summaryTemplateDto, ct);
    return NoContent();
  }

  [HttpGet]
  [Route("example")]
  public async Task<ActionResult<string>> GetExample(CancellationToken ct)
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
  public async Task<ActionResult<SummaryTemplateValidationResultDto>> Validate(
    [FromBody] SummaryTemplateDto summaryTemplateDto,
    CancellationToken ct)
  {
    var answer = await _summaryTemplateService.ValidateAsync(summaryTemplateDto, ct);
    return Ok(answer);
  }
}
