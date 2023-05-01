using VehicleGenius.Api.Services.Vehicles.VinAudit;

namespace VehicleGenius.Api.Dtos;

public class VehicleDto
{
  public Guid Id { get; set; }
  public string Vin { get; set; }
  public VinAuditData? VinAuditData { get; set; }
}
