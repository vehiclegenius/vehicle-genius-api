using VehicleGenius.Api.Dtos;

namespace VehicleGenius.Api.Services.PromptFeedback;

public interface IPromptFeedbackService
{
  Task<List<Models.Entities.PromptFeedback>> GetPromptFeedbacks(bool isResolved, CancellationToken ct);
  Task GivePromptFeedback(GivePromptFeedbackRequestDto requestDto);
  Task MarkPromptFeedbackAsResolved(Guid promptFeedbackId);
}
