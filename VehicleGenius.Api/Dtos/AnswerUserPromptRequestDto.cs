namespace VehicleGenius.Api.Dtos;

public class AnswerUserPromptRequestDto
{
  public string Username { get; set; }
  public Guid VehicleId { get; set; }
  public List<ChatMessageDto> Messages { get; set; }
}
