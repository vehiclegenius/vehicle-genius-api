namespace VehicleGenius.Api.Dtos;

public class AnswerUserPromptRequestDto
{
  public string Vin { get; set; }
  public List<ChatMessageDto> Messages { get; set; }
}
