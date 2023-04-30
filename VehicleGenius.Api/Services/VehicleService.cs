using Microsoft.EntityFrameworkCore;
using Scriban;
using Scriban.Runtime;
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

  public async Task<List<VehicleDto>> GetVehiclesAsync(CancellationToken ct)
  {
    var models = await GetQueryable().ToListAsync(ct);
    var dtos = models.Select(_vehicleMapperService.MapToDto).ToList();
    return dtos;
  }

  public async Task<string> GetVehicleSummaryAsync(Guid vehicleId, CancellationToken ct)
  {
    var vehicle = await GetQueryable().FirstOrDefaultAsync(v => v.Id == vehicleId, ct);
    var template = await _summaryTemplateService.GetForVersionAsync(1, ct);
    var liquidTemplate = Template.Parse(template.Template);
    var context = GetTemplateContext(vehicle);
    var interpolated = await liquidTemplate.RenderAsync(context);

    return interpolated;
  }

  private static TemplateContext GetTemplateContext(Vehicle? vehicle)
  {
    var specificationsAttributesScriptObject = new ScriptObject();
    specificationsAttributesScriptObject.Import(vehicle.VinAuditData.Specifications.Attributes,
      renamer: member => member.Name);

    var specificationsScriptObject = new ScriptObject();
    specificationsScriptObject.Add("Attributes", specificationsAttributesScriptObject);

    var scriptObject = new ScriptObject();
    scriptObject.Add("Specifications", specificationsScriptObject);
    
    var context = new TemplateContext();
    context.PushGlobal(scriptObject);

    return context;
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
