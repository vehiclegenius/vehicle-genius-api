namespace VehicleGenius.Api.Jobs;

public class SyncVehicleDataJob : IProgramJob
{
  public const string Name = "SyncVehicleData";
  
  public Task ExecuteJobAsync(string[] args)
  {
    return Task.CompletedTask;
    // For all vehicles
    //   Fetch data
  }
}
