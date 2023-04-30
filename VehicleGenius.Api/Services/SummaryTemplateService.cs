using Microsoft.EntityFrameworkCore;
using VehicleGenius.Api.Dtos;
using VehicleGenius.Api.Models;
using VehicleGenius.Api.Models.Entities;
using VehicleGenius.Api.Services.Mappers;

namespace VehicleGenius.Api.Services;

class SummaryTemplateService : ISummaryTemplateService
{
  private readonly VehicleGeniusDbContext _dbContext;
  private readonly IMapperService<SummaryTemplate, SummaryTemplateDto> _summaryTemplateMapperService;

  public SummaryTemplateService(VehicleGeniusDbContext dbContext, IMapperService<SummaryTemplate, SummaryTemplateDto> summaryTemplateMapperService)
  {
    _dbContext = dbContext;
    _summaryTemplateMapperService = summaryTemplateMapperService;
  }
  
  public async Task<SummaryTemplateDto> GetForVersionAsync(int version, CancellationToken ct)
  {
    return new SummaryTemplateDto
    {
      Id = Guid.NewGuid(),
      Template = @"Vehicle's specifications:
- Year: {{ Specifications.Attributes.Year }}
- Make: {{ Specifications.Attributes.Make }}
- Model: {{ Specifications.Attributes.Model }}
- Trim: {{ Specifications.Attributes.Trim }}
- Style: {{ Specifications.Attributes.Style }}
- Type: {{ Specifications.Attributes.Type }}
- Size: {{ Specifications.Attributes.Size }}
- Category: {{ Specifications.Attributes.Category }}
- Made in: {{ Specifications.Attributes.MadeIn }}
- Made in city: {{ Specifications.Attributes.MadeInCity }}
- Doors: {{ Specifications.Attributes.Doors }}
- Fuel type: {{ Specifications.Attributes.FuelType }}
- Fuel capacity: {{ Specifications.Attributes.FuelCapacity }}
- City mileage: {{ Specifications.Attributes.CityMileage }}
- Highway mileage: {{ Specifications.Attributes.HighwayMileage }}
- Engine: {{ Specifications.Attributes.Engine }}
- Engine size: {{ Specifications.Attributes.EngineSize }}
- Engine cylinders: {{ Specifications.Attributes.EngineCylinders }}
- Transmission: {{ Specifications.Attributes.Transmission }}
- Transmission type: {{ Specifications.Attributes.TransmissionType }}
- Transmission speeds: {{ Specifications.Attributes.TransmissionSpeeds }}
- Drivetrain: {{ Specifications.Attributes.Drivetrain }}
- Anti-brake system: {{ Specifications.Attributes.AntiBrakeSystem }}
- Steering type: {{ Specifications.Attributes.SteeringType }}
- Curb weight: {{ Specifications.Attributes.CurbWeight }}
- Gross vehicle weight rating: {{ Specifications.Attributes.GrossVehicleWeightRating }}
- Overall height: {{ Specifications.Attributes.OverallHeight }}
- Overall length: {{ Specifications.Attributes.OverallLength }}
- Overall width: {{ Specifications.Attributes.OverallWidth }}
- Wheelbase length: {{ Specifications.Attributes.WheelbaseLength }}
- Standard seating: {{ Specifications.Attributes.StandardSeating }}
- Invoice price: {{ Specifications.Attributes.InvoicePrice }}
- Delivery charges: {{ Specifications.Attributes.DeliveryCharges }}
- Manufacturer suggested retail price: {{ Specifications.Attributes.ManufacturerSuggestedRetailPrice }}

Vehicle's market value data:
- Estimated average market price: ${{ MarketValue.Mean }}
- Standard deviation of prices: ${{ MarketValue.Stdev }}
- Data points count: {{ MarketValue.Count }}
- Mileage used to compute: {{ MarketValue.Mileage }}
- Certainty: {{ MarketValue.Certainty }}%
- Period: {{ MarketValue.Period[0] }} to {{ MarketValue.Period[1] }}
- Prices:
  - Estimated average market price: ${{ MarketValue.Prices.Average }}
  - Estimated average below market price: ${{ MarketValue.Prices.Below }}
  - Estimated average above market price: ${{ MarketValue.Prices.Above }}

Vehicle's ownership cost data:
- Current mileage: {{ OwnershipCost.MileageStart }}
- Estimated yearly mileage: {{ OwnershipCost.MileageYear }}
- Estimated yearly depreciation cost for the next 5 years per year:
{{ for cost in OwnershipCost.DepreciationCost ~}}
  - ${{ cost }}
{{ end ~}}
- Estimated yearly insurance cost for the next 5 years per year:
{{ for cost in OwnershipCost.InsuranceCost ~}}
  - ${{ cost }}
{{ end ~}}
- Estimated yearly fuel cost for the next 5 years per year:
{{ for cost in OwnershipCost.FuelCost ~}}
  - ${{ cost }}
{{ end ~}}
- Estimated yearly maintenance cost for the next 5 years per year:
{{ for cost in OwnershipCost.MaintenanceCost ~}}
  - ${{ cost }}
{{ end ~}}
- Estimated yearly repairs cost for the next 5 years per year:
{{ for cost in OwnershipCost.RepairsCost ~}}
  - ${{ cost }}
{{ end ~}}
- Estimated yearly state fees & taxes cost for the next 5 years per year:
{{ for cost in OwnershipCost.FeesCost ~}}
  - ${{ cost }}
{{ end ~}}
- Estimated yearly cost for the next 5 years per year:
{{ for cost in OwnershipCost.TotalCost ~}}
  - ${{ cost }}
{{ end ~}}
- Estimated 5-year total cost: ${{ OwnershipCost.TotalCostSum }}",
    };

    // var summaryTemplate = await GetLastUpdatedForVersionAsync(version, ct);
    // return _summaryTemplateMapperService.MapToDto(summaryTemplate);
  }

  public async Task UpdateForVersionAsync(int version, SummaryTemplateDto dto, CancellationToken ct)
  {
    var summaryTemplate = await GetLastUpdatedForVersionAsync(version, ct);
    if (summaryTemplate is null)
    {
      summaryTemplate = new SummaryTemplate
      {
        VinAuditDataVersion = version,
        Template = dto.Template,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow,
      };
      _dbContext.SummaryTemplates.Add(summaryTemplate);
    }
    else
    {
      summaryTemplate.Template = dto.Template;
      summaryTemplate.UpdatedAt = DateTime.UtcNow;
    }
  }

  private async Task<SummaryTemplate?> GetLastUpdatedForVersionAsync(int version, CancellationToken ct)
  {
    return await _dbContext.SummaryTemplates
      .Where(st => st.VinAuditDataVersion == version)
      .OrderByDescending(st => st.UpdatedAt)
      .FirstOrDefaultAsync(ct);
  }
}
