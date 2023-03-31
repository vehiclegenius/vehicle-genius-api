using Microsoft.EntityFrameworkCore;
using VehicleGenius.Api.Models.Entities;

public class VehicleGeniusDbContext : DbContext
{
  public VehicleGeniusDbContext(DbContextOptions<VehicleGeniusDbContext> options)
    : base(options)
  {
  }

  public DbSet<Vehicle> Vehicles { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    modelBuilder.Entity<Vehicle>().HasKey(e => e.Id);
    base.OnModelCreating(modelBuilder);
  }
}
