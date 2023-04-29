using VehicleGenius.Api.Dtos;
using VehicleGenius.Api.Models.Entities;

namespace VehicleGenius.Api.Services.Mappers;

class VehicleMapperService : IMapperService<Vehicle, VehicleDto>
{
  public Vehicle MapToModel(VehicleDto dto)
  {
    return new Vehicle()
    {
      Id = dto.Id,
      Vin = dto.Vin,
    };
  }

  public VehicleDto MapToDto(Vehicle model)
  {
    return new VehicleDto
    {
      Id = model.Id,
      Vin = model.Vin,
      VinAuditData = model.VinAuditData,
    };
  }
}
