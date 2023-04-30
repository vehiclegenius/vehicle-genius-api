using VehicleGenius.Api.Dtos;
using VehicleGenius.Api.Services.VinAudit;

namespace VehicleGenius.Api.Services.AI;

public interface IAiService
{
  Task<List<ChatMessageDto>> GetAnswer(GetAnswerRequest request);
}
