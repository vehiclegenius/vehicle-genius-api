using VehicleGenius.Api.Services.AI;

namespace VehicleGenius.Api.Services;

public interface IVehicleService
{
  Task<object> GetVehicleInformation(UserPromptParts userPromptParts);
}
