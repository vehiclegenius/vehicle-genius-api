using System.ComponentModel.DataAnnotations.Schema;
using VehicleGenius.Api.Dtos;

namespace VehicleGenius.Api.Models.Entities;

public class PromptFeedback
{
  public Guid Id { get; set; }
  public Guid VehicleId { get; set; }
  public Vehicle? Vehicle { get; set; }
  public bool IsPositive { get; set; }
  public string? Reason { get; set; }
  
  [Column(TypeName = "jsonb")]
  public List<ChatMessageDto> Messages { get; set; }
  
  public DateTime CreatedAt { get; set; }
  public DateTime ResolvedAt { get; set; }
}
