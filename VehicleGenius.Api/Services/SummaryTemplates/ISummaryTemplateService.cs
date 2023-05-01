using VehicleGenius.Api.Dtos;
using VehicleGenius.Api.Models.Entities;

namespace VehicleGenius.Api.Services.SummaryTemplates;

public interface ISummaryTemplateService
{
  Task<SummaryTemplateDto> GetForVersionAsync(int version, CancellationToken ct);
  Task UpdateForVersionAsync(int version, SummaryTemplateDto dto, CancellationToken ct);
  Task<SummaryTemplateValidationResultDto> ValidateAsync(SummaryTemplateDto summaryTemplateDto, CancellationToken ct);
  Task<string> RenderTemplateAsync(SummaryTemplateDto template, Vehicle vehicle, CancellationToken ct);
}
