using System.ComponentModel.DataAnnotations.Schema;
using VehicleGenius.Api.Services.Vehicles.VinAudit;

namespace VehicleGenius.Api.Models.Entities;

public class Vehicle
{
  public Guid Id { get; set; }
  public string Vin { get; set; }
  
  [Column(TypeName = "jsonb")]
  public VinAuditData VinAuditData { get; set; }
  public int VinAuditDataVersion { get; set; }
  
  public ICollection<UserVehicle> UserVehicles { get; set; } = new List<UserVehicle>();
}
