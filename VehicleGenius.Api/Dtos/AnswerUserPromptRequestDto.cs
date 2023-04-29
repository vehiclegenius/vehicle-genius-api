namespace VehicleGenius.Api.Dtos;

public class AnswerUserPromptRequestDto
{
  public Guid VehicleId { get; set; }
  public List<ChatMessageDto> Messages { get; set; }
}
