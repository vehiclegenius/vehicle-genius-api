using Microsoft.EntityFrameworkCore;
using VehicleGenius.Api.Models.Entities;

namespace VehicleGenius.Api.Models;

public class VehicleGeniusDbContext : DbContext
{
  public VehicleGeniusDbContext(DbContextOptions<VehicleGeniusDbContext> options)
    : base(options)
  {
  }

  public DbSet<PromptFeedback> PromptFeedbacks { get; set; }
  public DbSet<SummaryTemplate> SummaryTemplates { get; set; }
  public DbSet<User> Users { get; set; }
  public DbSet<UserVehicle> UserVehicles { get; set; }
  public DbSet<Vehicle> Vehicles { get; set; }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);
    
    modelBuilder.Entity<UserVehicle>()
      .HasKey(uv => new { uv.UserId, uv.VehicleId });

    modelBuilder.Entity<UserVehicle>()
      .HasOne(uv => uv.User)
      .WithMany(u => u.UserVehicles)
      .HasForeignKey(uv => uv.UserId);

    modelBuilder.Entity<UserVehicle>()
      .HasOne(uv => uv.Vehicle)
      .WithMany(v => v.UserVehicles)
      .HasForeignKey(uv => uv.VehicleId);
  }
}
