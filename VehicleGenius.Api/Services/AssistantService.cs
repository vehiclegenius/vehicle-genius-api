using VehicleGenius.Api.Dtos;
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
  
  public async Task<List<ChatMessageDto>> AnswerUserPrompt(AnswerUserPromptRequestDto requestDto)
  {
    var vehicleSummary = await _vehicleService.GetVehicleSummaryAsync(requestDto.VehicleId, CancellationToken.None);

    var answer = await _aiService.GetAnswer(new GetAnswerRequest
    {
      Data = vehicleSummary,
      DataInFuture = false,
      Messages = requestDto.Messages,
    });

    return answer;
  }
}
