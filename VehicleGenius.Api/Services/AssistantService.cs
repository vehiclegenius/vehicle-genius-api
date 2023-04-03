using VehicleGenius.Api.Services.AI;

namespace VehicleGenius.Api.Services;

class AssistantService : IAssistantService
{
  private readonly IAiService _aiService;
  private readonly IVehicleService _vehicleService;

  public AssistantService(IAiService aiService, IVehicleService vehicleService)
  {
    _aiService = aiService;
    _vehicleService = vehicleService;
  }
  
  public async Task<string> AnswerUserPrompt(string prompt)
  {
    var promptParts = await _aiService.ParsePromptIntoParts(prompt);
    var vehicle = await _vehicleService.GetVehicleInformation(promptParts);
    var answer = await _aiService.BuildAnswer(vehicle);
    return answer;
  }
}
