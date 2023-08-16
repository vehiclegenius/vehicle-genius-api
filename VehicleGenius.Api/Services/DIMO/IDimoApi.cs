using VehicleGenius.Api.Services.DIMO.Models;

namespace VehicleGenius.Api.Services.DIMO;

public interface IDimoApi
{
  public Task<List<SharedDevice>> GetVehicleStatusesAsync(CancellationToken ct);
}
