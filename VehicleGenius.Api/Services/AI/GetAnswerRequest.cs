using VehicleGenius.Api.Dtos;

namespace VehicleGenius.Api.Services.AI;

public record GetAnswerRequest
{
  public object Data { get; set; }
  public bool DataInFuture { get; set; }
  public List<ChatMessageDto> Messages { get; set; }
};
