namespace VehicleGenius.Api.Services.VinAudit;

public interface IVinAuditService
{
  Task<VinAuditSpecificationsData> GetSpecifications(VinAuditPromptData vinAuditPromptData);
}
