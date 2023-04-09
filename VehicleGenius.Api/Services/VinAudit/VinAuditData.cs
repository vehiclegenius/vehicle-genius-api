namespace VehicleGenius.Api.Services.VinAudit;

public record VinAuditData
{
  public VinAuditMarketValueData MarketValue { get; set; }
  public VinAuditOwnershipCostData OwnershipCost { get; set; }
  public VinAuditSpecificationsData Specifications { get; set; }
};
