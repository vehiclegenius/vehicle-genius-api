namespace VehicleGenius.Api.Services;

public interface IAssistantService
{
  public Task<string> AnswerUserPrompt(string prompt);
}
