namespace VehicleGenius.Api.Models.Entities;

public class SummaryTemplate
{
  public Guid Id { get; set; }

  /// <summary>
  /// The "System" prompt for ChatGPT
  /// </summary>
  public string SystemPrompt { get; set; }

  /// <summary>
  /// The template for formatting the vehicle data
  /// </summary>
  public string DataTemplate { get; set; }

  /// <summary>
  /// The overall template of prompt sent to ChatGPT
  /// </summary>
  public string PromptTemplate { get; set; }

  public int VinAuditDataVersion { get; set; } = 1;
  public DateTime CreatedAt { get; set; }
  public DateTime UpdatedAt { get; set; }
}
