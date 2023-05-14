using Microsoft.AspNetCore.Mvc;
using VehicleGenius.Api.Dtos;
using VehicleGenius.Api.Services;
using VehicleGenius.Api.Services.PromptFeedback;
using VehicleGenius.Api.Services.Vehicles;

namespace VehicleGenius.Api.Controllers;

[ApiController]
[Route("assistant")]
public class AssistantController : ControllerBase
{
  private readonly IAssistantService _assistantService;
  private readonly IVehicleService _vehicleService;
  private readonly IPromptFeedbackService _promptFeedbackService;

  public AssistantController(IAssistantService assistantService, IVehicleService vehicleService, IPromptFeedbackService promptFeedbackService)
  {
    _assistantService = assistantService;
    _vehicleService = vehicleService;
    _promptFeedbackService = promptFeedbackService;
  }
  
  [HttpPost]
  [Route("prompt/answer")]
  [ProducesResponseType(typeof (List<ChatMessageDto>), StatusCodes.Status200OK)]
  public async Task<IActionResult> AnswerUserPrompt([FromBody] AnswerUserPromptRequestDto requestDto)
  {
    if (!await _vehicleService.VehicleExistsAsync(requestDto.VehicleId, CancellationToken.None))
    {
      return NotFound();
    }
    
    var answer = await _assistantService.AnswerUserPrompt(requestDto);

    return Ok(answer);
  }
  
  [HttpPost]
  [Route("prompt/feedback")]
  [ProducesResponseType(StatusCodes.Status204NoContent)]
  public async Task<IActionResult> GivePromptFeedback([FromBody] GivePromptFeedbackRequestDto requestDto)
  {
    if (!await _vehicleService.VehicleExistsAsync(requestDto.VehicleId, CancellationToken.None))
    {
      return NoContent();
    }
    
    await _promptFeedbackService.GivePromptFeedback(requestDto);

    return NoContent();
  }
}
