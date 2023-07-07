namespace VehicleGenius.Api.Jobs;

public interface IProgramJobFactory
{
  IProgramJob Create(string jobName);
}
