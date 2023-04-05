using Flurl;
using Flurl.Http;

namespace VehicleGenius.Api.Services.VinAudit;

class VinAuditService : IVinAuditService
{
  private readonly HttpClient _httpClient;
  private readonly string _apiKey;

  public VinAuditService(IConfiguration configuration)
  {
    _apiKey = configuration.GetValue<string>("VinAudit:ApiKey")!;
  }

  public async Task<VinAuditSpecificationsData> GetSpecifications(VinAuditPromptData vinAuditPromptData)
  {
    var specifications = await "https://specifications.vinaudit.com/v3/specifications"
      .SetQueryParams(new
      {
        key = _apiKey,
        vin = vinAuditPromptData.Vin,
        format = "json",
        include = "attributes",
      })
      .GetJsonAsync<VinAuditSpecificationsData>();
    return specifications;
  }
}
