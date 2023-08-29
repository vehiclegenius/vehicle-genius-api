using Microsoft.EntityFrameworkCore;
using VehicleGenius.Api.Models;
using VehicleGenius.Api.Models.Entities;
using VehicleGenius.Api.Services.DIMO;
using VehicleGenius.Api.Services.Vehicles;

namespace VehicleGenius.Api.Jobs;

public class SyncVehicleStatusJob : IProgramJob
{
  public const string Name = "SyncVehicleStatus";
  private readonly VehicleGeniusDbContext _dbContext;
  private readonly IDimoApi _dimoApi;
  private readonly IVehicleService _vehicleService;

  public SyncVehicleStatusJob(VehicleGeniusDbContext dbContext, IDimoApi dimoApi, IVehicleService vehicleService)
  {
    _dbContext = dbContext;
    _dimoApi = dimoApi;
    _vehicleService = vehicleService;
  }

  public async Task ExecuteJobAsync(string[] args)
  {
    Console.WriteLine("Syncing vehicle status...");

    var sharedVehicles = await _dimoApi.GetVehicleStatusesAsync(CancellationToken.None);

    foreach (var sharedVehicle in sharedVehicles)
    {
      Console.WriteLine($"Syncing vehicle status for {sharedVehicle.Vin}...");
      var vehicle = await _dbContext.Vehicles.FirstOrDefaultAsync(v => v.Vin == sharedVehicle.Vin);

      if (vehicle == null)
      {
        Console.WriteLine($"Creating new vehicle for {sharedVehicle.Vin}...");
        vehicle = new Vehicle
        {
          Id = Guid.NewGuid(),
          Vin = sharedVehicle.Vin,
          DimoVehicleStatus = sharedVehicle.DeviceStatus,
        };
        _dbContext.Vehicles.Add(vehicle);
      }
      else
      {
        Console.WriteLine($"Updating vehicle status for {sharedVehicle.Vin}...");
        vehicle.DimoVehicleStatus = sharedVehicle.DeviceStatus;
      }

      _vehicleService.AssignVehicleToUserAsync(_dbContext, sharedVehicle.OwnerAddress, vehicle.Id);
    }

    await _dbContext.SaveChangesAsync(CancellationToken.None);

    Console.WriteLine("Done syncing vehicle status.");
  }
}
