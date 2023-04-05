using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace VehicleGenius.Api.Services.VinAudit;

public class VinAuditSpecificationsData
{
  [JsonProperty(PropertyName = "input")] public VinAuditSpecificationsDataInputData? Input { get; set; }

  [JsonProperty(PropertyName = "attributes")] public VinAuditSpecificationsDataAttributesData? Attributes { get; set; }

  [JsonProperty(PropertyName = "success")] public bool Success { get; set; }

  [JsonProperty(PropertyName = "error")] public string Error { get; set; }
}

public class VinAuditSpecificationsDataInputData
{
  [Required] [JsonProperty(PropertyName = "key")] public string Key { get; set; }

  [Required] [JsonProperty(PropertyName = "vin")] public string Vin { get; set; }

  [Required]
  [JsonProperty(PropertyName = "format")]
  public string Format { get; set; }

  [Required]
  [JsonProperty(PropertyName = "include")]
  public string Include { get; set; }
}

public class VinAuditSpecificationsDataAttributesData
{
  [Required] [JsonProperty(PropertyName = "year")] public string Year { get; set; }

  [Required] [JsonProperty(PropertyName = "make")] public string Make { get; set; }

  [Required] [JsonProperty(PropertyName = "model")] public string Model { get; set; }

  [Required] [JsonProperty(PropertyName = "trim")] public string Trim { get; set; }

  [JsonProperty(PropertyName = "style")] public string Style { get; set; }

  [Required] [JsonProperty(PropertyName = "type")] public string Type { get; set; }

  [Required] [JsonProperty(PropertyName = "size")] public string Size { get; set; }

  [Required]
  [JsonProperty(PropertyName = "category")]
  public string Category { get; set; }

  [Required]
  [JsonProperty(PropertyName = "made_in")]
  public string MadeIn { get; set; }

  [Required]
  [JsonProperty(PropertyName = "made_in_city")]
  public string MadeInCity { get; set; }

  [Required] [JsonProperty(PropertyName = "doors")] public string Doors { get; set; }

  [Required]
  [JsonProperty(PropertyName = "fuel_type")]
  public string FuelType { get; set; }

  [Required]
  [JsonProperty(PropertyName = "fuel_capacity")]
  public string FuelCapacity { get; set; }

  [Required]
  [JsonProperty(PropertyName = "city_mileage")]
  public string CityMileage { get; set; }

  [Required]
  [JsonProperty(PropertyName = "highway_mileage")]
  public string HighwayMileage { get; set; }

  [Required]
  [JsonProperty(PropertyName = "engine")]
  public string Engine { get; set; }

  [Required]
  [JsonProperty(PropertyName = "engine_size")]
  public string EngineSize { get; set; }

  [Required]
  [JsonProperty(PropertyName = "engine_cylinders")]
  public string EngineCylinders { get; set; }

  [Required]
  [JsonProperty(PropertyName = "transmission")]
  public string Transmission { get; set; }

  [Required]
  [JsonProperty(PropertyName = "transmission_type")]
  public string TransmissionType { get; set; }

  [Required]
  [JsonProperty(PropertyName = "transmission_speeds")]
  public string TransmissionSpeeds { get; set; }

  [Required]
  [JsonProperty(PropertyName = "drivetrain")]
  public string Drivetrain { get; set; }

  [Required]
  [JsonProperty(PropertyName = "anti_brake_system")]
  public string AntiBrakeSystem { get; set; }

  [Required]
  [JsonProperty(PropertyName = "steering_type")]
  public string SteeringType { get; set; }

  [Required]
  [JsonProperty(PropertyName = "curb_weight")]
  public string CurbWeight { get; set; }

  [JsonProperty(PropertyName = "gross_vehicle_weight_rating")]
  public string GrossVehicleWeightRating { get; set; }

  [Required]
  [JsonProperty(PropertyName = "overall_height")]
  public string OverallHeight { get; set; }

  [Required]
  [JsonProperty(PropertyName = "overall_length")]
  public string OverallLength { get; set; }

  [Required]
  [JsonProperty(PropertyName = "overall_width")]
  public string OverallWidth { get; set; }

  [Required]
  [JsonProperty(PropertyName = "wheelbase_length")]
  public string WheelbaseLength { get; set; }

  [Required]
  [JsonProperty(PropertyName = "standard_seating")]
  public string StandardSeating { get; set; }

  [Required]
  [JsonProperty(PropertyName = "invoice_price")]
  public string InvoicePrice { get; set; }

  [Required]
  [JsonProperty(PropertyName = "delivery_charges")]
  public string DeliveryCharges { get; set; }

  [Required]
  [JsonProperty(PropertyName = "manufacturer_suggested_retail_price")]
  public string ManufacturerSuggestedRetailPrice { get; set; }
}
