namespace VehicleGenius.Api.Services.VinAudit;

public interface IVinAuditService
{
  Task<VinAuditData> GetVinAuditData(VinAuditPromptData vinAuditPromptData);
}
