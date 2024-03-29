using VehicleGenius.Api.Services.DIMO;
using VehicleGenius.Api.Services.Vehicles.VinAudit;

namespace VehicleGenius.Api.Dtos;

public class VehicleDto
{
  public Guid Id { get; set; }
  public string Vin { get; set; }
  public VinAuditData? VinAuditData { get; set; }
  public VehicleUserDataDto? UserData { get; set; }
  public DimoVehicleStatus? DeviceStatus { get; set; }
}
