using VehicleGenius.Api.Dtos;
using VehicleGenius.Api.Services.Mappers;

namespace VehicleGenius.Api.Services.PromptFeedback;

public class PromptFeedbackMapperService : IMapperService<Models.Entities.PromptFeedback, PromptFeedbackDto>
{
  public Models.Entities.PromptFeedback MapToModel(PromptFeedbackDto dto)
  {
    throw new NotImplementedException();
  }

  public PromptFeedbackDto MapToDto(Models.Entities.PromptFeedback model)
  {
    return new PromptFeedbackDto
    {
      Id = model.Id,
      IsPositive = false,
      Reason = model.Reason,
      Messages = model.Messages,
      CreatedAt = model.CreatedAt,
      ResolvedAt = model.ResolvedAt,
    };
  }
}
