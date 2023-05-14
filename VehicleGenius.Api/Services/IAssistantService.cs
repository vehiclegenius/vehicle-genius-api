using VehicleGenius.Api.Dtos;

namespace VehicleGenius.Api.Services;

public interface IAssistantService
{
  public Task<List<ChatMessageDto>> AnswerUserPrompt(AnswerUserPromptRequestDto requestDto);
}
