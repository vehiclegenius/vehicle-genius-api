using Microsoft.AspNetCore.Mvc;
using VehicleGenius.Api.Services;

namespace VehicleGenius.Api.Controllers;

[ApiController]
[Route("assistant")]
public class AssistantController : ControllerBase
{
  private readonly IAssistantService _assistantService;

  public AssistantController(IAssistantService assistantService)
  {
    _assistantService = assistantService;
  }
  
  [HttpPost]
  public async Task<string> AnswerUserPrompt([FromBody] string prompt)
  {
    var answer = await _assistantService.AnswerUserPrompt(prompt);
    return answer;
  }
}
