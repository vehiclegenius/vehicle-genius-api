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

  public SyncVehicleStatusJob(VehicleGeniusDbContext dbContext, IDimoApi dimoApi)
  {
    _dbContext = dbContext;
    _dimoApi = dimoApi;
  }

  public async Task ExecuteJobAsync(string[] args)
  {
    Console.WriteLine("Syncing vehicle status...");

    var sharedVehicles = await _dimoApi.GetVehicleStatusesAsync(CancellationToken.None);
    
    foreach (var sharedVehicle in sharedVehicles)
    {
      var vehicle = await _dbContext.Vehicles.FirstOrDefaultAsync(v => v.Vin == sharedVehicle.Vin);
      
      if (vehicle == null)
      {
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
        vehicle.DimoVehicleStatus = sharedVehicle.DeviceStatus;
      }
    }

    await _dbContext.SaveChangesAsync(CancellationToken.None);

    Console.WriteLine("Done syncing vehicle status.");
  }
}
