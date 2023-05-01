namespace VehicleGenius.Api.Services.Vehicles.VinAudit;

public interface IVinAuditService
{
  Task<VinAuditData> GetVinAuditData(VinAuditPromptData vinAuditPromptData);
}
