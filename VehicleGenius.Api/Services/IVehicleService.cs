using VehicleGenius.Api.Dtos;
using VehicleGenius.Api.Models.Entities;

namespace VehicleGenius.Api.Services;

public interface IVehicleService
{
  Task<bool> VehicleExistsAsync(Guid vehicleId, CancellationToken ct);
  Task<List<VehicleDto>> GetVehiclesAsync(CancellationToken ct);
  Task<VehicleDto> GetSingleVehicleAsync(Guid vehicleId, CancellationToken ct);
  Task<string> GetVehicleSummaryAsync(Guid vehicleId, CancellationToken ct);
  Task UpdateVehicleAsync(VehicleDto vehicleDto);
}
