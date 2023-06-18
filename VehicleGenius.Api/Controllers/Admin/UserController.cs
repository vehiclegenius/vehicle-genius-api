using Microsoft.AspNetCore.Mvc;
using VehicleGenius.Api.Dtos;
using VehicleGenius.Api.Services.Users;
using VehicleGenius.Api.Services.Vehicles;

namespace VehicleGenius.Api.Controllers.Admin;

[ApiController]
[Route("admin/users")]
public class UserController : ControllerBase
{
  private readonly IUserService _userService;
  private readonly IVehicleService _vehicleService;

  public UserController(IUserService userService, IVehicleService vehicleService)
  {
    _userService = userService;
    _vehicleService = vehicleService;
  }
  
  [HttpGet]
  [Route("")]
  [ProducesResponseType(typeof(List<UserDto>), StatusCodes.Status200OK)]
  public async Task<IActionResult> GetUsers(CancellationToken ct)
  {
    return Ok(await _userService.GetUsers(ct));
  }
  
  [HttpGet]
  [Route("{username}/vehicles/{vehicleId}")]
  [ProducesResponseType(typeof(VehicleDto), StatusCodes.Status200OK)]
  public async Task<IActionResult> GetVehicle(string username, Guid vehicleId, CancellationToken ct)
  {
    return Ok(await _vehicleService.GetSingleVehicleAsync(vehicleId, ct));
  }
}
