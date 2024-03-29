namespace VehicleGenius.Api.Jobs;

public class ProgramJobFactory : IProgramJobFactory
{
  private readonly IServiceProvider _serviceProvider;

  public ProgramJobFactory(IServiceProvider serviceProvider)
  {
    _serviceProvider = serviceProvider;
  }

  public IProgramJob Create(string jobName)
  {
    return jobName switch
    {
      SyncVehicleDataJob.Name => _serviceProvider.GetRequiredService<SyncVehicleDataJob>(),
      SyncVehicleStatusJob.Name => _serviceProvider.GetRequiredService<SyncVehicleStatusJob>(),
      GetAccessTokensJob.Name => _serviceProvider.GetRequiredService<GetAccessTokensJob>(),
      _ => throw new ArgumentException($"Invalid job name: {jobName}")
    };
  }
}
