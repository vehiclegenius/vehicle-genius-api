using VehicleGenius.Api.Dtos;
using VehicleGenius.Api.Models.Entities;
using VehicleGenius.Api.Services.Mappers;

namespace VehicleGenius.Api.Services.Vehicles;

class VehicleMapperService : IMapperService<Vehicle, VehicleDto>
{
  public Vehicle MapToModel(VehicleDto dto)
  {
    return new Vehicle()
    {
      Id = dto.Id,
      Vin = dto.Vin,
      UserData = dto.UserData,
    };
  }

  public VehicleDto MapToDto(Vehicle model)
  {
    return new VehicleDto
    {
      Id = model.Id,
      Vin = model.Vin,
      VinAuditData = model.VinAuditData,
      UserData = model.UserData,
      DeviceStatus = model.DimoVehicleStatus,
    };
  }
}
