using Microsoft.EntityFrameworkCore;
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

  public SummaryTemplateService(VehicleGeniusDbContext dbContext, IMapperService<SummaryTemplate, SummaryTemplateDto> summaryTemplateMapperService)
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

  private async Task<SummaryTemplate?> GetLastUpdatedForVersionAsync(int version, CancellationToken ct)
  {
    return await _dbContext.SummaryTemplates
      .Where(st => st.VinAuditDataVersion == version)
      .OrderByDescending(st => st.UpdatedAt)
      .FirstOrDefaultAsync(ct);
  }
}
