namespace VehicleGenius.Api.Dtos;

public class UserDto
{
  public string Username { get; init; }
  public List<UserVehicleDto> Vehicles { get; set; }
}
