using VehicleGenius.Api.Dtos;

namespace VehicleGenius.Api.Services;

public interface IAssistantService
{
  public Task<string> AnswerUserPrompt(AnswerUserPromptRequestDto requestDto);
}
