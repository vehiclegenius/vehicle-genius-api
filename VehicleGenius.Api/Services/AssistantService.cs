using VehicleGenius.Api.Dtos;
using VehicleGenius.Api.Models;
using VehicleGenius.Api.Models.Entities;
using VehicleGenius.Api.Services.AI;
using VehicleGenius.Api.Services.Vehicles;

namespace VehicleGenius.Api.Services;

class AssistantService : IAssistantService
{
  private readonly VehicleGeniusDbContext _dbContext;
  private readonly IAiService _aiService;
  private readonly IVehicleService _vehicleService;

  public AssistantService(VehicleGeniusDbContext dbContext, IAiService aiService, IVehicleService vehicleService)
  {
    _dbContext = dbContext;
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

  public async Task GivePromptFeedback(GivePromptFeedbackRequestDto requestDto)
  {
    var promptFeedback = new PromptFeedback
    {
      VehicleId = requestDto.VehicleId,
      IsPositive = requestDto.IsPositive,
      Reason = requestDto.Reason,
      Messages = requestDto.Messages,
      CreatedAt = DateTime.UtcNow,
    };
    
    _dbContext.PromptFeedbacks.Add(promptFeedback);
    
    await _dbContext.SaveChangesAsync();
  }
}
