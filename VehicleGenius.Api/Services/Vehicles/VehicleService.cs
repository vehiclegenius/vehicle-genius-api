using Microsoft.EntityFrameworkCore;
using VehicleGenius.Api.Dtos;
using VehicleGenius.Api.Models;
using VehicleGenius.Api.Models.Entities;
using VehicleGenius.Api.Services.DIMO;
using VehicleGenius.Api.Services.Mappers;
using VehicleGenius.Api.Services.SummaryTemplates;
using VehicleGenius.Api.Services.Vehicles.VinAudit;

namespace VehicleGenius.Api.Services.Vehicles;

class VehicleService : IVehicleService
{
  private readonly VehicleGeniusDbContext _dbContext;
  private readonly IMapperService<Vehicle, VehicleDto> _vehicleMapperService;
  private readonly IVinAuditService _vinAuditService;
  private readonly ISummaryTemplateService _summaryTemplateService;
  private readonly IDimoApi _dimoApi;

  public VehicleService(
    VehicleGeniusDbContext dbContext,
    IMapperService<Vehicle, VehicleDto> vehicleMapperService,
    IVinAuditService vinAuditService,
    ISummaryTemplateService summaryTemplateService,
    IDimoApi dimoApi)
  {
    _dbContext = dbContext;
    _vehicleMapperService = vehicleMapperService;
    _vinAuditService = vinAuditService;
    _summaryTemplateService = summaryTemplateService;
    _dimoApi = dimoApi;
  }

  public async Task<bool> VehicleExistsAsync(string vin, CancellationToken ct)
  {
    return await _dbContext.Vehicles.AnyAsync(v => v.Vin == vin, ct);
  }

  public async Task<bool> VehicleExistsAsync(Guid vehicleId, CancellationToken ct)
  {
    return await _dbContext.Vehicles.AnyAsync(v => v.Id == vehicleId, ct);
  }

  public async Task<bool> UserOwnsVehicleAsync(Guid vehicleId, string username, CancellationToken ct)
  {
    return await _dbContext.Vehicles.AnyAsync(
      v => v.Id == vehicleId &&
           v.UserVehicles.Any(uv => uv.User.Username.ToLower() == username.ToLower()),
      ct);
  }

  public async Task<List<VehicleDto>> GetVehiclesAsync(string username, CancellationToken ct)
  {
    var models = await GetQueryable()
      .Where(v => v.UserVehicles.Any(uv => uv.User.Username.ToLower() == username.ToLower()))
      .ToListAsync(ct);
    var dtos = models.Select(_vehicleMapperService.MapToDto).ToList();
    return dtos;
  }

  public async Task<VehicleDto> GetSingleVehicleAsync(Guid vehicleId, CancellationToken ct)
  {
    var vehicle = await GetQueryable().FirstOrDefaultAsync(v => v.Id == vehicleId, ct);
    return _vehicleMapperService.MapToDto(vehicle);
  }

  public async Task<string> GetVehicleSummaryAsync(Guid vehicleId, CancellationToken ct)
  {
    var vehicle = await GetQueryable().FirstOrDefaultAsync(v => v.Id == vehicleId, ct);
    var template = await _summaryTemplateService.GetForVersionAsync(1, ct);
    var interpolated = await _summaryTemplateService.RenderTemplateAsync(template, vehicle, ct);

    return interpolated;
  }

  public async Task<Vehicle> UpsertVehicleAsync(VehicleDto vehicleDto)
  {
    var model = _vehicleMapperService.MapToModel(vehicleDto);

    var existingVehicle = await _dbContext.Vehicles
      .AsNoTracking()
      .FirstOrDefaultAsync(v => v.Id == model.Id || v.Vin == model.Vin);

    // If the ID exists, then we're updating an existing vehicle.  Update it
    // with select new data.
    // If the ID doesn't exist, but the VIN does, then we're trying to insert a
    // duplicate.  Duplicate the existing vehicle with freshly fetched data and
    // save.
    // Otherwise, we're inserting a new vehicle.  Save it.

    if (existingVehicle?.Id == model.Id)
    {
      existingVehicle.UserData = model.UserData;
      existingVehicle.UpdatedAt = DateTime.UtcNow;
      _dbContext.Update(existingVehicle);
    }
    else
    {
      await PopulateVehicleWithLatestDataAsync(_dbContext, model, CancellationToken.None);
      model.UserData = new VehicleUserDataDto();
      _dbContext.Add(model);
    }

    await _dbContext.SaveChangesAsync();

    return model;
  }

  public void AssignVehicleToUserAsync(VehicleGeniusDbContext context, string username, Guid vehicleId)
  {
    var user = _dbContext.Users
      .Include(u => u.UserVehicles)
      .FirstOrDefault(u => u.Username == username);

    if (user == null)
    {
      var userId = Guid.NewGuid();
      user = new User
      {
        Id = userId,
        Username = username,
        UserVehicles = new List<UserVehicle>()
        {
          new()
          {
            UserId = userId,
            VehicleId = vehicleId,
          },
        },
      };
      _dbContext.Add(user);
    }
    else if (user.UserVehicles.All(uv => uv.VehicleId != vehicleId))
    {
      user.UserVehicles.Add(new UserVehicle
      {
        UserId = user.Id,
        VehicleId = vehicleId,
      });
      _dbContext.Update(user);
    }
  }

  public async Task AssignVehicleToUserAsync(string username, Guid vehicleId)
  {
    AssignVehicleToUserAsync(_dbContext, username, vehicleId);
    await _dbContext.SaveChangesAsync();
  }

  public async Task SyncVehicleDataAsync(Guid vehicleId, CancellationToken ct)
  {
    var vehicle = await GetQueryable()
      .AsTracking()
      .FirstOrDefaultAsync(v => v.Id == vehicleId, ct);

    if (vehicle == null)
    {
      return;
    }

    await PopulateVehicleWithLatestDataAsync(_dbContext, vehicle, ct);

    await _dbContext.SaveChangesAsync();
  }

  private async Task PopulateVehicleWithLatestDataAsync(VehicleGeniusDbContext context, Vehicle vehicle, CancellationToken ct)
  {
    vehicle.DataUpdatedAt = DateTime.UtcNow;
    vehicle.VinAuditData = await _vinAuditService.GetVinAuditData(
      new VinAuditPromptData() { Vin = vehicle.Vin });
  }

  private IQueryable<Vehicle> GetQueryable()
  {
    return _dbContext.Vehicles;
  }

  async Task IVehicleService.FetchDimoDataAsync(string vin, string username, CancellationToken ct)
  {
    var sharedDevice = await _dimoApi.GetVehicleStatusAsync(vin, ct);

    var vehicle = await GetQueryable()
      .FirstOrDefaultAsync(v => v.Vin == vin, ct);

    if (vehicle == null)
    {
      vehicle = new Vehicle
      {
        Id = Guid.NewGuid(),
        Vin = vin,
        DimoVehicleStatus = sharedDevice.DeviceStatus,
      };
      _dbContext.Add(vehicle);
    }
    else
    {
      vehicle.DimoVehicleStatus = sharedDevice.DeviceStatus;
      _dbContext.Update(vehicle);
    }

    await AssignVehicleToUserAsync(sharedDevice.OwnerAddress, vehicle.Id);

    await _dbContext.SaveChangesAsync();
  }
}
