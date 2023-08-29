using VehicleGenius.Api.Services.DIMO.Models;

namespace VehicleGenius.Api.Services.DIMO;

public interface IDimoApi
{
  public Task<string> GetDimoAccessToken(CancellationToken ct);
  public Task<SharedDevice> GetVehicleStatusAsync(string vin, CancellationToken ct);
  public Task<List<SharedDevice>> GetVehicleStatusesAsync(CancellationToken ct);
}
