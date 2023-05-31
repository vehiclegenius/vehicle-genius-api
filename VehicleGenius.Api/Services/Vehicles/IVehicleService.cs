using VehicleGenius.Api.Dtos;

namespace VehicleGenius.Api.Services.Vehicles;

public interface IVehicleService
{
  Task<bool> VehicleExistsAsync(Guid vehicleId, CancellationToken ct);
  Task<bool> UserOwnsVehicleAsync(Guid vehicleId, string username, CancellationToken ct);
  Task<List<VehicleDto>> GetVehiclesAsync(string username, CancellationToken ct);
  Task<VehicleDto> GetSingleVehicleAsync(Guid vehicleId, CancellationToken ct);
  Task<string> GetVehicleSummaryAsync(Guid vehicleId, CancellationToken ct);
  Task UpdateVehicleAsync(VehicleDto vehicleDto);
  Task AssignVehicleToUserAsync(string username, Guid vehicleId);
}
