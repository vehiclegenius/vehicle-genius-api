namespace VehicleGenius.Api.Startup;

public class StartupServiceManager : IStartupServiceManager
{
  private readonly IEnumerable<IStartupService> _startupServices;

  public StartupServiceManager(IEnumerable<IStartupService> startupServices)
  {
    _startupServices = startupServices;
  }

  public async Task StartAsync()
  {
    foreach (var startupService in _startupServices.OrderBy(s => s.Priority))
    {
      var name = startupService.GetType().Name;
      try
      {
        Console.WriteLine($"Starting service {name}");
        await startupService.StartAsync(CancellationToken.None);
      }
      catch (Exception e)
      {
        Console.WriteLine($"Startup of the application failed with service {name}", e);
        throw;
      }
    }
  }
}
