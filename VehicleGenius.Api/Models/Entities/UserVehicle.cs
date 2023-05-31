using Microsoft.EntityFrameworkCore;

namespace VehicleGenius.Api.Models.Entities;

[Keyless]
public class UserVehicle
{
  public Guid UserId { get; set; }
  public User User { get; set; }
  public Guid VehicleId { get; set; }
  public Vehicle Vehicle { get; set; }
}
