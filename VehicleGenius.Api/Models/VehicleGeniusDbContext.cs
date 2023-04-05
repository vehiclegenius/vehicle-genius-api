using Microsoft.EntityFrameworkCore;

namespace VehicleGenius.Api.Models;

public class VehicleGeniusDbContext : DbContext
{
  public VehicleGeniusDbContext(DbContextOptions<VehicleGeniusDbContext> options)
    : base(options)
  {
  }
}
