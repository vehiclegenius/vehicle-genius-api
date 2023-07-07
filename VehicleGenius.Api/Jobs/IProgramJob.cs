namespace VehicleGenius.Api.Jobs;

public interface IProgramJob
{
  Task ExecuteJobAsync(string[] args);
}
