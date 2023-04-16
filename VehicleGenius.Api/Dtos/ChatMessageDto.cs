namespace VehicleGenius.Api.Dtos;

public record ChatMessageDto
{
  public string Role { get; set; }
  public string Content { get; set; }
}
