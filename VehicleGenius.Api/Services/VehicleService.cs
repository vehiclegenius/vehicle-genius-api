using Microsoft.EntityFrameworkCore;
using Scriban;
using VehicleGenius.Api.Dtos;
using VehicleGenius.Api.Models;
using VehicleGenius.Api.Models.Entities;
using VehicleGenius.Api.Services.Mappers;
using VehicleGenius.Api.Services.VinAudit;

namespace VehicleGenius.Api.Services;

class VehicleService : IVehicleService
{
  private readonly VehicleGeniusDbContext _dbContext;
  private readonly IMapperService<Vehicle, VehicleDto> _vehicleMapperService;
  private readonly IVinAuditService _vinAuditService;

  public VehicleService(
    VehicleGeniusDbContext dbContext,
    IMapperService<Vehicle, VehicleDto> vehicleMapperService,
    IVinAuditService vinAuditService)
  {
    _dbContext = dbContext;
    _vehicleMapperService = vehicleMapperService;
    _vinAuditService = vinAuditService;
  }

  public async Task<bool> VehicleExistsAsync(Guid vehicleId, CancellationToken ct)
  {
    return await _dbContext.Vehicles.AnyAsync(v => v.Id == vehicleId, ct);
  }

  public async Task<List<VehicleDto>> GetVehiclesAsync(CancellationToken ct)
  {
    var models = await GetQueryable().ToListAsync(ct);
    var dtos = models.Select(_vehicleMapperService.MapToDto).ToList();
    return dtos;
  }

  public async Task<string> GetVehicleSummaryAsync(Guid vehicleId, CancellationToken ct)
  {
    var vehicle = await GetQueryable().FirstOrDefaultAsync(v => v.Id == vehicleId, ct);
    var template = await _dbContext.SummaryTemplates.FirstOrDefaultAsync(ct);
    var liquidTemplate = Template.Parse(template!.Template);

    var interpolated = await liquidTemplate.RenderAsync(new
    {
      VinAuditMarketValue = vehicle.VinAuditData.MarketValue,
      VinAuditOwnershipCost = vehicle.VinAuditData.OwnershipCost,
      VinAuditSpecifications = vehicle.VinAuditData.Specifications,
    });

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

  private IQueryable<Vehicle> GetQueryable()
  {
    return _dbContext.Vehicles;
  }
}
