using Microsoft.EntityFrameworkCore;
using VehicleGenius.Api.Dtos;
using VehicleGenius.Api.Models;
using VehicleGenius.Api.Models.Entities;
using VehicleGenius.Api.Services.Mappers;

namespace VehicleGenius.Api.Services.Users;

class UserService : IUserService
{
  private readonly VehicleGeniusDbContext _dbContext;
  private readonly IMapperService<User, UserDto> _userMapperService;

  public UserService(VehicleGeniusDbContext dbContext, IMapperService<User, UserDto> userMapperService)
  {
    _dbContext = dbContext;
    _userMapperService = userMapperService;
  }

  public async Task<List<UserDto>> GetUsers(CancellationToken ct)
  {
    var users = await GetQueryable().ToListAsync(ct);
    return users.Select(_userMapperService.MapToDto).ToList();
  }

  private IQueryable<User> GetQueryable()
  {
    return _dbContext.Users
      .Include(u => u.UserVehicles)
      .ThenInclude(uv => uv.Vehicle)
      .OrderBy(u => u.Username);
  }
}
