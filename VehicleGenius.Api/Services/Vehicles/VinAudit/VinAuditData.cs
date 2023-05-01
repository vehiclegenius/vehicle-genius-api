namespace VehicleGenius.Api.Services.Vehicles.VinAudit;

public record VinAuditData
{
  public VinAuditMarketValueData MarketValue { get; set; }
  public VinAuditOwnershipCostData OwnershipCost { get; set; }
  public VinAuditSpecificationsData Specifications { get; set; }
};
