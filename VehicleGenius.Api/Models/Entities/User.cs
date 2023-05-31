using System.ComponentModel.DataAnnotations.Schema;

namespace VehicleGenius.Api.Models.Entities;

public class User
{
  public Guid Id { get; set; }
  public string Username { get; set; }
  public ICollection<UserVehicle> UserVehicles { get; set; } = new List<UserVehicle>();
}
