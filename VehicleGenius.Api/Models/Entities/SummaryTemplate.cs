namespace VehicleGenius.Api.Models.Entities;

public class SummaryTemplate
{
  public Guid Id { get; set; }
  public string Template { get; set; }
  public int VinAuditDataVersion { get; set; } = 1;
}
