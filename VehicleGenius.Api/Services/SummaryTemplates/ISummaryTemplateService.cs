using VehicleGenius.Api.Dtos;

namespace VehicleGenius.Api.Services.SummaryTemplates;

public interface ISummaryTemplateService
{
  Task<SummaryTemplateDto> GetForVersionAsync(int version, CancellationToken ct);
  Task UpdateForVersionAsync(int version, SummaryTemplateDto dto, CancellationToken ct);
}
