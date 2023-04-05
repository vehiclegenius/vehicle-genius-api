using VehicleGenius.Api.Models.Entities;
using VehicleGenius.Api.Services.AI;
using VehicleGenius.Api.Services.VinAudit;

namespace VehicleGenius.Api.Services;

class AssistantService : IAssistantService
{
  private readonly IAiService _aiService;
  private readonly IVinAuditService _vinAuditService;

  public AssistantService(IAiService aiService, IVinAuditService vinAuditService)
  {
    _aiService = aiService;
    _vinAuditService = vinAuditService;
  }
  
  public async Task<string> AnswerUserPrompt(string prompt)
  {
    var vinAuditUserData = new VinAuditPromptData
    {
      Vin = "JTME6RFV0ND522512",
    };
    var queryTopicApi = await _aiService.GetQueryTopicApi(prompt);
    
    switch (queryTopicApi)
    {
      case QueryTopicApi.VinAuditSpecifications:
        var specifications = await _vinAuditService.GetSpecifications(vinAuditUserData);
        return await _aiService.GetAnswer(specifications, prompt);
      case QueryTopicApi.VinAuditMarketValue:
        return "foo";
      case QueryTopicApi.VinAuditOwnershipCost:
        return "foo";
      case QueryTopicApi.None:
      default:
        throw new Exception("No appropriate API found for prompt.");
    }
  }
}
