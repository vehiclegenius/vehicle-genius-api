using VehicleGenius.Api.Dtos;

namespace VehicleGenius.Api.Services.Users;

public interface IUserService
{
  Task<List<UserDto>> GetUsers(CancellationToken ct);
}
