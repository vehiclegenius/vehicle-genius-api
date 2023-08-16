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
  private readonly string _defaultSystemPrompt = "You are a helpful assistant.";

  private readonly string _defaultPromptTemplate = @"With this data:

{Data}

{UserMessage}";

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
        SystemPrompt = _defaultSystemPrompt,
        DataTemplate = _defaultTemplate,
        PromptTemplate = _defaultPromptTemplate,
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
        SystemPrompt = dto.SystemPrompt,
        DataTemplate = dto.DataTemplate,
        PromptTemplate = dto.PromptTemplate,
        UpdatedAt = DateTime.UtcNow,
      };
      _dbContext.SummaryTemplates.Add(summaryTemplate);
    }
    else
    {
      summaryTemplate.SystemPrompt = dto.SystemPrompt;
      summaryTemplate.DataTemplate = dto.DataTemplate;
      summaryTemplate.PromptTemplate = dto.PromptTemplate;
      summaryTemplate.UpdatedAt = DateTime.UtcNow;
    }

    await _dbContext.SaveChangesAsync(ct);
  }

  public async Task<SummaryTemplateValidationResultDto> ValidateAsync(
    SummaryTemplateDto summaryTemplateDto,
    CancellationToken ct)
  {
    try
    {
      var liquidTemplate = Template.Parse(summaryTemplateDto.DataTemplate);

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
    var liquidTemplate = Template.Parse(template.DataTemplate);
    var context = GetTemplateContext(vehicle);
    var interpolated = await liquidTemplate.RenderAsync(context);
    return interpolated;
  }

  private static TemplateContext GetTemplateContext(Vehicle? vehicle)
  {
    var scriptObject = new ScriptObject
    {
      { "Specifications", GetSpecificationsScriptObject(vehicle) },
      { "MarketValue", GetMarketValueScriptObject(vehicle) },
      { "OwnershipCost", GetOwnershipCostsScriptObject(vehicle) },
      { "UserData", GetUserDataScriptObject(vehicle) },
      { "DeviceStatus", GetDeviceStatusScriptObject(vehicle) },
    };

    var context = new TemplateContext();
    context.PushGlobal(scriptObject);

    return context;
  }

  private static ScriptObject GetSpecificationsScriptObject(Vehicle? vehicle)
  {
    var specificationsAttributesScriptObject = new ScriptObject();
    specificationsAttributesScriptObject.Import(vehicle.VinAuditData.Specifications.Attributes,
      renamer: ScriptObjectRenamer());

    var specificationsScriptObject = new ScriptObject();
    specificationsScriptObject.Add("Attributes", specificationsAttributesScriptObject);

    return specificationsScriptObject;
  }

  private static ScriptObject GetMarketValueScriptObject(Vehicle vehicle)
  {
    var marketValuePricesScriptObject = new ScriptObject();
    marketValuePricesScriptObject.Import(vehicle.VinAuditData.MarketValue.Prices, renamer: ScriptObjectRenamer());

    var marketValueScriptObject = new ScriptObject();
    marketValueScriptObject.Import(vehicle.VinAuditData.MarketValue, renamer: ScriptObjectRenamer());
    marketValueScriptObject.Remove("Prices");
    marketValueScriptObject.Add("Prices", marketValuePricesScriptObject);

    return marketValueScriptObject;
  }

  private static ScriptObject GetOwnershipCostsScriptObject(Vehicle vehicle)
  {
    var scriptObject = new ScriptObject();
    scriptObject.Import(vehicle.VinAuditData.OwnershipCost, renamer: ScriptObjectRenamer());
    return scriptObject;
  }

  private static object GetUserDataScriptObject(Vehicle vehicle)
  {
    var scriptObject = new ScriptObject();
    scriptObject.Import(vehicle.UserData, renamer: ScriptObjectRenamer());
    return scriptObject;
  }

  private static object GetDeviceStatusScriptObject(Vehicle vehicle)
  {
    var scriptObject = new ScriptObject();
    scriptObject.Import(vehicle.DimoVehicleStatus, renamer: ScriptObjectRenamer());
    return scriptObject;
  }

  private static MemberRenamerDelegate ScriptObjectRenamer()
  {
    return member => member.Name;
  }

  private async Task<SummaryTemplate?> GetLastUpdatedForVersionAsync(int version, CancellationToken ct)
  {
    return await _dbContext.SummaryTemplates
      .Where(st => st.VinAuditDataVersion == version)
      .OrderByDescending(st => st.UpdatedAt)
      .FirstOrDefaultAsync(ct);
  }
}
