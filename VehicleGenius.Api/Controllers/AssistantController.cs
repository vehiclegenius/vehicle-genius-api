using Microsoft.AspNetCore.Mvc;
using VehicleGenius.Api.Dtos;
using VehicleGenius.Api.Services;

namespace VehicleGenius.Api.Controllers;

[ApiController]
[Route("assistant")]
public class AssistantController : ControllerBase
{
  private readonly IAssistantService _assistantService;
  private readonly IVehicleService _vehicleService;

  public AssistantController(IAssistantService assistantService, IVehicleService vehicleService)
  {
    _assistantService = assistantService;
    _vehicleService = vehicleService;
  }
  
  [HttpPost]
  [Route("answer-user-prompt")]
  public async Task<IActionResult> AnswerUserPrompt([FromBody] AnswerUserPromptRequestDto requestDto)
  {
    if (!await _vehicleService.VehicleExistsAsync(requestDto.VehicleId, CancellationToken.None))
    {
      return NotFound();
    }
    
    var answer = await _assistantService.AnswerUserPrompt(requestDto);

    return Ok(answer);
  }
}
