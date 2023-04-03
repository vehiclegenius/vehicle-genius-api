namespace VehicleGenius.Api.Services.AI;

public interface IAiService
{
  Task<UserPromptParts> ParsePromptIntoParts(string prompt);
  Task<string> BuildAnswer(object vehicle);
}
