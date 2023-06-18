namespace VehicleGenius.Api.Dtos;

public class UserDto
{
  public string Username { get; init; }
  public List<UserVehicleDto> Vehicles { get; set; }
}

public class UserVehicleDto
{
  public Guid Id { get; set; }
  public string Vin { get; set; }
  public string Name { get; set; }
}
