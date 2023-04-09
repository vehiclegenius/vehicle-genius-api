using Flurl;
using Flurl.Http;

namespace VehicleGenius.Api.Services.VinAudit;

class VinAuditService : IVinAuditService
{
  private readonly string _apiKey;

  public VinAuditService(IConfiguration configuration)
  {
    _apiKey = configuration.GetValue<string>("VinAudit:ApiKey")!;
  }

  public async Task<VinAuditData> GetVinAuditData(VinAuditPromptData vinAuditPromptData)
  {
    return new VinAuditData
    {
      Specifications = await GetSpecifications(vinAuditPromptData),
      MarketValue = await GetMarketValue(vinAuditPromptData),
      OwnershipCost = await GetOwnershipCost(vinAuditPromptData),
    };
  }

  private async Task<VinAuditSpecificationsData> GetSpecifications(VinAuditPromptData vinAuditPromptData)
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

  private async Task<VinAuditMarketValueData> GetMarketValue(VinAuditPromptData vinAuditPromptData)
  {
    var marketValue = await "http://marketvalue.vinaudit.com/getmarketvalue.php"
      .SetQueryParams(new
      {
        key = _apiKey,
        vin = vinAuditPromptData.Vin,
        format = "json",
        mileage = "average",
        period = 90,
      })
      .GetJsonAsync<VinAuditMarketValueData>();
    return marketValue;
  }

  private async Task<VinAuditOwnershipCostData> GetOwnershipCost(VinAuditPromptData vinAuditPromptData)
  {
    var ownershipCost = await "http://ownershipcost.vinaudit.com/getownershipcost.php"
      .SetQueryParams(new
      {
        key = _apiKey,
        vin = vinAuditPromptData.Vin,
        format = "json",
        state = "WA",
      })
      .GetJsonAsync<VinAuditOwnershipCostData>();
    return ownershipCost;
  }
}
