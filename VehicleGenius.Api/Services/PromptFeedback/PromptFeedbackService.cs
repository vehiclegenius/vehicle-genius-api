using Microsoft.EntityFrameworkCore;
using VehicleGenius.Api.Dtos;
using VehicleGenius.Api.Models;

namespace VehicleGenius.Api.Services.PromptFeedback;

class PromptFeedbackService : IPromptFeedbackService
{
  private readonly VehicleGeniusDbContext _dbContext;

  public PromptFeedbackService(VehicleGeniusDbContext dbContext)
  {
    _dbContext = dbContext;
  }

  public Task<List<Models.Entities.PromptFeedback>> GetPromptFeedbacks(bool isResolved, CancellationToken ct)
  {
    var queryable = isResolved ? GetResolvedPromptFeedbacks() : GetUnresolvedPromptFeedbacks();
    return queryable.ToListAsync(ct);
  }

  private IQueryable<Models.Entities.PromptFeedback> GetUnresolvedPromptFeedbacks()
  {
    return GetQueryable().Where(pf => !pf.ResolvedAt.HasValue);
  }

  private IQueryable<Models.Entities.PromptFeedback> GetResolvedPromptFeedbacks()
  {
    return GetQueryable().Where(pf => pf.ResolvedAt.HasValue);
  }

  public async Task GivePromptFeedback(GivePromptFeedbackRequestDto requestDto)
  {
    var promptFeedback = new Models.Entities.PromptFeedback
    {
      VehicleId = requestDto.VehicleId,
      IsPositive = requestDto.IsPositive,
      Reason = requestDto.Reason,
      Messages = requestDto.Messages,
      CreatedAt = DateTime.UtcNow,
    };

    _dbContext.PromptFeedbacks.Add(promptFeedback);

    await _dbContext.SaveChangesAsync();
  }

  public async Task MarkPromptFeedbackAsResolved(Guid promptFeedbackId)
  {
    var promptFeedback = await _dbContext.PromptFeedbacks.FirstOrDefaultAsync(pf => pf.Id == promptFeedbackId);
    
    if (promptFeedback == null)
    {
      return;
    }
    
    promptFeedback.ResolvedAt = DateTime.UtcNow;
    
    await _dbContext.SaveChangesAsync();
  }

  private IQueryable<Models.Entities.PromptFeedback> GetQueryable()
  {
    return _dbContext.PromptFeedbacks
      .Include(pf => pf.Vehicle)
      .OrderByDescending(s => s.CreatedAt);
  }
}
