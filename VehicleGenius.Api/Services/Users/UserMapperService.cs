using VehicleGenius.Api.Dtos;
using VehicleGenius.Api.Models.Entities;
using VehicleGenius.Api.Services.Mappers;

namespace VehicleGenius.Api.Services.Users;

public class UserMapperService : IMapperService<User, UserDto>
{
  private readonly IMapperService<UserVehicle, UserVehicleDto> _userVehicleMapperService;

  public UserMapperService(IMapperService<UserVehicle, UserVehicleDto> userVehicleMapperService)
  {
    _userVehicleMapperService = userVehicleMapperService;
  }
  
  public User MapToModel(UserDto dto)
  {
    throw new NotImplementedException();
  }

  public UserDto MapToDto(User model)
  {
    return new UserDto
    {
      Username = model.Username,
      Vehicles = model.UserVehicles.Select(_userVehicleMapperService.MapToDto).ToList(),
    };
  }
}
