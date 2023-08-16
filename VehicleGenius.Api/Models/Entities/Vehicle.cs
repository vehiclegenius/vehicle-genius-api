using System.ComponentModel.DataAnnotations.Schema;
using VehicleGenius.Api.Dtos;
using VehicleGenius.Api.Services.DIMO;
using VehicleGenius.Api.Services.Vehicles.VinAudit;

namespace VehicleGenius.Api.Models.Entities;

public class Vehicle
{
  public Guid Id { get; set; }
  public string Vin { get; set; }

  [Column(TypeName = "jsonb")]
  public VinAuditData? VinAuditData { get; set; }

  [Column(TypeName = "jsonb")]
  public VehicleUserDataDto? UserData { get; set; } = new();
  
  [Column(TypeName = "jsonb")]
  public DimoVehicleStatus? DimoVehicleStatus { get; set; }

  public ICollection<UserVehicle> UserVehicles { get; set; } = new List<UserVehicle>();

  public DateTime UpdatedAt { get;set; }
  public DateTime DataUpdatedAt { get; set; }
}
