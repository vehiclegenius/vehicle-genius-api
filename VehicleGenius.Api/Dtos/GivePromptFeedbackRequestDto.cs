namespace VehicleGenius.Api.Dtos;

public class GivePromptFeedbackRequestDto
{
  public string Username { get; set; }
  public Guid VehicleId { get; set; }
  public List<ChatMessageDto> Messages { get; set; }
  public bool IsPositive { get; set; }
  public string? Reason { get; set; }
}
