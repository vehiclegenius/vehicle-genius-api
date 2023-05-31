using Microsoft.EntityFrameworkCore;
using VehicleGenius.Api.Dtos;
using VehicleGenius.Api.Models;
using VehicleGenius.Api.Models.Entities;
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

  public VehicleService(
    VehicleGeniusDbContext dbContext,
    IMapperService<Vehicle, VehicleDto> vehicleMapperService,
    IVinAuditService vinAuditService,
    ISummaryTemplateService summaryTemplateService)
  {
    _dbContext = dbContext;
    _vehicleMapperService = vehicleMapperService;
    _vinAuditService = vinAuditService;
    _summaryTemplateService = summaryTemplateService;
  }

  public async Task<bool> VehicleExistsAsync(Guid vehicleId, CancellationToken ct)
  {
    return await _dbContext.Vehicles.AnyAsync(v => v.Id == vehicleId, ct);
  }

  public async Task<bool> UserOwnsVehicleAsync(Guid vehicleId, string username, CancellationToken ct)
  {
    return await _dbContext.Vehicles.AnyAsync(
      v => v.Id == vehicleId &&
           v.UserVehicles.Any(uv => uv.User.Username == username),
      ct);
  }

  public async Task<List<VehicleDto>> GetVehiclesAsync(string username, CancellationToken ct)
  {
    var models = await GetQueryable().Where(v => v.UserVehicles.Any(uv => uv.User.Username == username)).ToListAsync(ct);
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

  public async Task UpdateVehicleAsync(VehicleDto vehicleDto)
  {
    var model = _vehicleMapperService.MapToModel(vehicleDto);

    if (await _dbContext.Vehicles.AnyAsync(v => v.Id == model.Id))
    {
      _dbContext.Update(model);
    }
    else
    {
      model.VinAuditData = await _vinAuditService.GetVinAuditData(new VinAuditPromptData() { Vin = model.Vin });
      model.VinAuditDataVersion = 1;
      _dbContext.Add(model);
    }

    await _dbContext.SaveChangesAsync();
  }

  public async Task AssignVehicleToUserAsync(string username, Guid vehicleId)
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
    
    await _dbContext.SaveChangesAsync();
  }

  private IQueryable<Vehicle> GetQueryable()
  {
    return _dbContext.Vehicles;
  }
}
