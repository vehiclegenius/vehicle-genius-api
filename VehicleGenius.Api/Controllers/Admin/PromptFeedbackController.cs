using Microsoft.AspNetCore.Mvc;
using VehicleGenius.Api.Dtos;
using VehicleGenius.Api.Models.Entities;
using VehicleGenius.Api.Services.Mappers;
using VehicleGenius.Api.Services.PromptFeedback;

namespace VehicleGenius.Api.Controllers.Admin;

[ApiController]
[Route("admin/prompt-feedback")]
public class PromptFeedbackController : ControllerBase
{
  private readonly IPromptFeedbackService _promptFeedbackService;
  private readonly IMapperService<PromptFeedback, PromptFeedbackDto> _promptFeedbackMapperService;

  public PromptFeedbackController(
    IPromptFeedbackService promptFeedbackService,
    IMapperService<PromptFeedback, PromptFeedbackDto> promptFeedbackMapperService)
  {
    _promptFeedbackService = promptFeedbackService;
    _promptFeedbackMapperService = promptFeedbackMapperService;
  }

  [HttpGet]
  [Route("")]
  [ProducesResponseType(typeof(List<PromptFeedbackDto>), StatusCodes.Status200OK)]
  public async Task<IActionResult> GetPromptFeedbacks([FromQuery] bool isResolved, CancellationToken ct)
  {
    var records = await _promptFeedbackService.GetPromptFeedbacks(isResolved, ct);
    var dtos = records.Select(_promptFeedbackMapperService.MapToDto).ToList();
    return Ok(dtos);
  }

  [HttpPost]
  [Route("{id}/resolve")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<IActionResult> MarkPromptFeedbackAsResolved(Guid id)
  {
    await _promptFeedbackService.MarkPromptFeedbackAsResolved(id);
    return NoContent();
  }
}
