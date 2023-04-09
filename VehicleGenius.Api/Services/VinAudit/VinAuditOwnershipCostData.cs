namespace VehicleGenius.Api.Services.VinAudit;

using Newtonsoft.Json;

public class VinAuditOwnershipCostData
{
  [JsonProperty(PropertyName = "vin")]
  public string Vin { get; set; }

  [JsonProperty(PropertyName = "mileage_start")]
  public int MileageStart { get; set; }

  [JsonProperty(PropertyName = "mileage_year")]
  public int MileageYear { get; set; }

  [JsonProperty(PropertyName = "success")]
  public bool Success { get; set; }

  [JsonProperty(PropertyName = "vehicle")]
  public string Vehicle { get; set; }

  [JsonProperty(PropertyName = "depreciation_cost")]
  public int[] DepreciationCost { get; set; }

  [JsonProperty(PropertyName = "insurance_cost")]
  public int[] InsuranceCost { get; set; }

  [JsonProperty(PropertyName = "fuel_cost")]
  public int[] FuelCost { get; set; }

  [JsonProperty(PropertyName = "maintenance_cost")]
  public int[] MaintenanceCost { get; set; }

  [JsonProperty(PropertyName = "repairs_cost")]
  public int[] RepairsCost { get; set; }

  [JsonProperty(PropertyName = "fees_cost")]
  public int[] FeesCost { get; set; }

  [JsonProperty(PropertyName = "total_cost")]
  public int[] TotalCost { get; set; }

  [JsonProperty(PropertyName = "total_cost_sum")]
  public int TotalCostSum { get; set; }
}

