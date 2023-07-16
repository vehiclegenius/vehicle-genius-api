namespace VehicleGenius.Api.Services.DIMO;

public interface IDimoApi
{
  public Task<List<DimoVehicleStatus>> GetVehicleStatusesAsync(CancellationToken ct);
}
