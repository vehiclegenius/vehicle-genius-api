using VehicleGenius.Api.Dtos;
using VehicleGenius.Api.Models.Entities;
using VehicleGenius.Api.Services.Mappers;

namespace VehicleGenius.Api.Services.Users;

public class UserVehicleMapperService : IMapperService<UserVehicle, UserVehicleDto>
{
  public UserVehicle MapToModel(UserVehicleDto dto)
  {
    throw new NotImplementedException();
  }

  public UserVehicleDto MapToDto(UserVehicle model)
  {
    return new UserVehicleDto
    {
      Id = model.VehicleId,
      Vin = model.Vehicle.Vin,
      Name = model.Vehicle.VinAuditData.MarketValue.Vehicle,
    };
  }
}
