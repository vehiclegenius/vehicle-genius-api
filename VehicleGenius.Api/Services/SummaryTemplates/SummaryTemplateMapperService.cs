using VehicleGenius.Api.Dtos;
using VehicleGenius.Api.Models.Entities;
using VehicleGenius.Api.Services.Mappers;

namespace VehicleGenius.Api.Services.SummaryTemplates;

public class SummaryTemplateMapperService : IMapperService<SummaryTemplate, SummaryTemplateDto>
{
  public SummaryTemplate MapToModel(SummaryTemplateDto dto)
  {
    throw new NotImplementedException();
  }

  public SummaryTemplateDto MapToDto(SummaryTemplate model)
  {
    return new SummaryTemplateDto
    {
      Template = model.Template,
    };
  }
}
