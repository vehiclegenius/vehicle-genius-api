using Microsoft.EntityFrameworkCore;
using VehicleGenius.Api.Models;
using VehicleGenius.Api.Services.Vehicles;

namespace VehicleGenius.Api.Jobs;

public class SyncVehicleDataJob : IProgramJob
{
  public const string Name = "SyncVehicleData";
  private readonly VehicleGeniusDbContext _dbContext;
  private readonly IVehicleService _vehicleService;

  public SyncVehicleDataJob(VehicleGeniusDbContext dbContext, IVehicleService vehicleService)
  {
    _dbContext = dbContext;
    _vehicleService = vehicleService;
  }

  public async Task ExecuteJobAsync(string[] args)
  {
    Console.WriteLine("Syncing vehicle data...");

    var vehicles = await _dbContext.Vehicles
      .AsNoTracking()
      .Select(v => new { v.Id, v.Vin })
      .ToListAsync();

    // TODO eventually move this to a queue
    foreach (var vehicle in vehicles) {
      Console.WriteLine($"Syncing vehicle {vehicle.Id} with VIN {vehicle.Vin}");
      await _vehicleService.SyncVehicleDataAsync(vehicle.Id, CancellationToken.None);
    }

    Console.WriteLine("Done syncing vehicle data.");
  }
}
