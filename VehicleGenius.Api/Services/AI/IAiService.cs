using VehicleGenius.Api.Dtos;

namespace VehicleGenius.Api.Services.AI;

public interface IAiService
{
  Task<List<ChatMessageDto>> GetAnswer(GetAnswerRequest request);
}
