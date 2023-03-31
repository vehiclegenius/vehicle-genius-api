namespace VehicleGenius.Api.Startup;

public interface IStartupService
{
  public int Priority { get; }
  public Task StartAsync(CancellationToken cancellationToken);
}
