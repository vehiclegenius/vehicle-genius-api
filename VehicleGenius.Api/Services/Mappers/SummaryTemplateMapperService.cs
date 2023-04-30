using VehicleGenius.Api.Dtos;
using VehicleGenius.Api.Models.Entities;

namespace VehicleGenius.Api.Services.Mappers;

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
      Id = model.Id,
      Template = model.Template,
    };
  }
}
