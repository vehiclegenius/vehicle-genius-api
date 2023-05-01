namespace VehicleGenius.Api.Services.Vehicles.VinAudit;

using Newtonsoft.Json;
using System.Collections.Generic;

public class VinAuditMarketValueData
{
    [JsonProperty(PropertyName = "vin")]
    public string Vin { get; set; }
    
    [JsonProperty(PropertyName = "success")]
    public bool Success { get; set; }
    
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }
    
    [JsonProperty(PropertyName = "vehicle")]
    public string Vehicle { get; set; }
    
    [JsonProperty(PropertyName = "mean")]
    public double Mean { get; set; }
    
    [JsonProperty(PropertyName = "stdev")]
    public int Stdev { get; set; }
    
    [JsonProperty(PropertyName = "count")]
    public int Count { get; set; }
    
    [JsonProperty(PropertyName = "mileage")]
    public int Mileage { get; set; }
    
    [JsonProperty(PropertyName = "certainty")]
    public int Certainty { get; set; }
    
    [JsonProperty(PropertyName = "period")]
    public List<string> Period { get; set; }
    
    [JsonProperty(PropertyName = "prices")]
    public VinAuditMarketValueDataPricesData Prices { get; set; }
    
    [JsonProperty(PropertyName = "adjustments")]
    public VinAuditMarketValueDataAdjustmentsData Adjustments { get; set; }
}

public class VinAuditMarketValueDataPricesData
{
    [JsonProperty(PropertyName = "average")]
    public double Average { get; set; }
    
    [JsonProperty(PropertyName = "below")]
    public double Below { get; set; }
    
    [JsonProperty(PropertyName = "above")]
    public double Above { get; set; }
    
    [JsonProperty(PropertyName = "distribution")]
    public List<VinAuditMarketValueDataDistributionData> Distribution { get; set; }
}

public class VinAuditMarketValueDataDistributionData
{
    [JsonProperty(PropertyName = "group")]
    public VinAuditMarketValueDataGroupData Group { get; set; }
}

public class VinAuditMarketValueDataGroupData
{
    [JsonProperty(PropertyName = "min")]
    public int Min { get; set; }
    
    [JsonProperty(PropertyName = "max")]
    public int Max { get; set; }
    
    [JsonProperty(PropertyName = "count")]
    public int Count { get; set; }
}

public class VinAuditMarketValueDataAdjustmentsData
{
    [JsonProperty(PropertyName = "mileage")]
    public VinAuditMarketValueDataMileageData Mileage { get; set; }
}

public class VinAuditMarketValueDataMileageData
{
    [JsonProperty(PropertyName = "average")]
    public double Average { get; set; }
    
    [JsonProperty(PropertyName = "input")]
    public double Input { get; set; }
    
    [JsonProperty(PropertyName = "adjustment")]
    public int Adjustment { get; set; }
}

