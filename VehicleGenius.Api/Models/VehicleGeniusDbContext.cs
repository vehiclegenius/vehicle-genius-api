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
  public DbSet<Vehicle> Vehicles { get; set; }
}
