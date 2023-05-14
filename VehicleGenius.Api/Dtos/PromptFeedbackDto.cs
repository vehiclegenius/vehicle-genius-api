namespace VehicleGenius.Api.Dtos;

public class PromptFeedbackDto
{
  public Guid Id { get; set; }
  public bool IsPositive = false;
  public string? Reason { get; set; }
  public List<ChatMessageDto> Messages { get; set; }
  public DateTime CreatedAt { get; set; }
  public DateTime? ResolvedAt { get; set; }
}
