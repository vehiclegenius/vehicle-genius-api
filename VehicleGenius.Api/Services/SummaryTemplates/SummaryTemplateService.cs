using Microsoft.EntityFrameworkCore;
using Scriban;
using Scriban.Runtime;
using VehicleGenius.Api.Dtos;
using VehicleGenius.Api.Models;
using VehicleGenius.Api.Models.Entities;
using VehicleGenius.Api.Services.Mappers;

namespace VehicleGenius.Api.Services.SummaryTemplates;

class SummaryTemplateService : ISummaryTemplateService
{
  private readonly VehicleGeniusDbContext _dbContext;
  private readonly IMapperService<SummaryTemplate, SummaryTemplateDto> _summaryTemplateMapperService;
  private readonly string _defaultTemplate;

  public SummaryTemplateService(
    VehicleGeniusDbContext dbContext,
    IMapperService<SummaryTemplate, SummaryTemplateDto> summaryTemplateMapperService)
  {
    _dbContext = dbContext;
    _summaryTemplateMapperService = summaryTemplateMapperService;
    _defaultTemplate = File.ReadAllText("Services/SummaryTemplates/DefaultTemplate.liquid");
  }

  public async Task<SummaryTemplateDto> GetForVersionAsync(int version, CancellationToken ct)
  {
    var summaryTemplate = await GetLastUpdatedForVersionAsync(version, ct);

    if (summaryTemplate is null)
    {
      summaryTemplate = new SummaryTemplate()
      {
        Id = Guid.NewGuid(),
        VinAuditDataVersion = version,
        Template = _defaultTemplate,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow,
      };
      _dbContext.SummaryTemplates.Add(summaryTemplate);
      await _dbContext.SaveChangesAsync(ct);
    }

    return _summaryTemplateMapperService.MapToDto(summaryTemplate);
  }

  public async Task UpdateForVersionAsync(int version, SummaryTemplateDto dto, CancellationToken ct)
  {
    var summaryTemplate = await GetLastUpdatedForVersionAsync(version, ct);
    if (summaryTemplate is null)
    {
      summaryTemplate = new SummaryTemplate
      {
        VinAuditDataVersion = version,
        Template = dto.Template,
        UpdatedAt = DateTime.UtcNow,
      };
      _dbContext.SummaryTemplates.Add(summaryTemplate);
    }
    else
    {
      summaryTemplate.Template = dto.Template;
      summaryTemplate.UpdatedAt = DateTime.UtcNow;
    }
  }

  public async Task<SummaryTemplateValidationResultDto> ValidateAsync(
    SummaryTemplateDto summaryTemplateDto,
    CancellationToken ct)
  {
    try
    {
      var liquidTemplate = Template.Parse(summaryTemplateDto.Template);
      
      if (liquidTemplate.HasErrors)
      {
        return new SummaryTemplateValidationResultDto
        {
          IsValid = false,
          ErrorMessage = liquidTemplate.Messages.First().ToString(),
          Preview = "",
        };
      }
      
      var vehicle = await _dbContext.Vehicles.FirstOrDefaultAsync(ct);

      return new SummaryTemplateValidationResultDto()
      {
        IsValid = true,
        ErrorMessage = "",
        Preview = await RenderTemplateAsync(summaryTemplateDto, vehicle, ct),
      };
    }
    catch (Exception ex)
    {
      return new SummaryTemplateValidationResultDto
      {
        IsValid = false,
        ErrorMessage = ex.Message,
        Preview = "",
      };
    }
    
  }

  public async Task<string> RenderTemplateAsync(SummaryTemplateDto template, Vehicle vehicle, CancellationToken ct)
  {
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
    
    var ownershipCostsScriptObject = new ScriptObject();
    ownershipCostsScriptObject.Import(vehicle.VinAuditData.OwnershipCost,
      renamer: member => member.Name);
    
    var marketValuePricesScriptObject = new ScriptObject();
    marketValuePricesScriptObject.Import(vehicle.VinAuditData.MarketValue.Prices,
      renamer: member => member.Name);
    
    var marketValueScriptObject = new ScriptObject();
    marketValueScriptObject.Import(vehicle.VinAuditData.MarketValue,
      renamer: member => member.Name);
    marketValueScriptObject.Remove("Prices");
    marketValueScriptObject.Add("Prices", marketValuePricesScriptObject);

    var scriptObject = new ScriptObject();
    scriptObject.Add("Specifications", specificationsScriptObject);
    scriptObject.Add("MarketValue", marketValueScriptObject);
    scriptObject.Add("OwnershipCost", ownershipCostsScriptObject);
    
    var context = new TemplateContext();
    context.PushGlobal(scriptObject);

    return context;
  }

  private async Task<SummaryTemplate?> GetLastUpdatedForVersionAsync(int version, CancellationToken ct)
  {
    return await _dbContext.SummaryTemplates
      .Where(st => st.VinAuditDataVersion == version)
      .OrderByDescending(st => st.UpdatedAt)
      .FirstOrDefaultAsync(ct);
  }
}
