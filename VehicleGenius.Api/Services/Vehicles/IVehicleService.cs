using VehicleGenius.Api.Dtos;
using VehicleGenius.Api.Models;
using VehicleGenius.Api.Models.Entities;

namespace VehicleGenius.Api.Services.Vehicles;

public interface IVehicleService
{
  Task<bool> VehicleExistsAsync(Guid vehicleId, CancellationToken ct);
  Task<bool> VehicleExistsAsync(string vehicleVin, CancellationToken ct);
  Task<bool> UserOwnsVehicleAsync(Guid vehicleId, string username, CancellationToken ct);
  Task<List<VehicleDto>> GetVehiclesAsync(string username, CancellationToken ct);
  Task<VehicleDto> GetSingleVehicleAsync(Guid vehicleId, CancellationToken ct);
  Task<string> GetVehicleSummaryAsync(Guid vehicleId, CancellationToken ct);
  Task<Vehicle> UpsertVehicleAsync(VehicleDto vehicleDto);
  void AssignVehicleToUserAsync(VehicleGeniusDbContext context, string username, Guid vehicleId);
  Task AssignVehicleToUserAsync(string username, Guid vehicleId);
  Task SyncVehicleDataAsync(Guid vehicleId, CancellationToken ct);
  Task FetchDimoDataAsync(string vin, string username, CancellationToken ct);
}
