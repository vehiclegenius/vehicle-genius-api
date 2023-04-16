using Microsoft.AspNetCore.Mvc;
using VehicleGenius.Api.Dtos;
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
  [Route("answer-user-prompt")]
  public async Task<List<ChatMessageDto>> AnswerUserPrompt([FromBody] AnswerUserPromptRequestDto requestDto)
  {
    var answer = await _assistantService.AnswerUserPrompt(requestDto);
    return answer;
  }
}
