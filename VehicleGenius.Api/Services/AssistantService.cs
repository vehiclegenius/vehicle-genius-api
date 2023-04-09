using VehicleGenius.Api.Dtos;
using VehicleGenius.Api.Models.Entities;
using VehicleGenius.Api.Services.AI;
using VehicleGenius.Api.Services.VinAudit;

namespace VehicleGenius.Api.Services;

class AssistantService : IAssistantService
{
  private readonly IAiService _aiService;
  private readonly IVinAuditService _vinAuditService;

  public AssistantService(IAiService aiService, IVinAuditService vinAuditService)
  {
    _aiService = aiService;
    _vinAuditService = vinAuditService;
  }
  
  public async Task<string> AnswerUserPrompt(AnswerUserPromptRequestDto requestDto)
  {
    var vinAuditUserData = new VinAuditPromptData
    {
      Vin = requestDto.Vin,
    };

    string summary;

    // Here just to speed up the entire query during the MVP stage
    if (requestDto.Vin == "JTME6RFV0ND522512")
    {
      summary = @"With this data:

Here is the list of vehicle specifications:

- Year: 2022
- Make: Toyota
- Model: RAV4 Hybrid
- Trim: XLE
- Style: 
- Type: Sport Utility Vehicle
- Size: 
- Category: Standard Sport Utility Vehicle
- Made in: United States
- Made in city: Georgetown
- Doors: 4-Door
- Fuel type: Regular
- Fuel capacity: 14.50 gallons
- City mileage: 41 miles/gallon
- Highway mileage: 38 miles/gallon
- Engine: 2.5-L L-4 DOHC 16V Hybrid
- Engine size: 2.5
- Engine cylinders: 4
- Transmission: Continuously Variable Transmission
- Transmission type: Electronic Continuously Variable E-CVT
- Transmission speeds: 
- Drivetrain: All-Wheel Drive
- Anti-brake system: 4-Wheel ABS
- Steering type: Rack & Pinion
- Curb weight: 
- Gross vehicle weight rating: 5000 pounds
- Overall height: 
- Overall length: 180.90 inches
- Overall width: 
- Wheelbase length: 105.90 inches
- Standard seating: 5
- Invoice price: $29,927
- Delivery charges: $1,215
- Manufacturer suggested retail price: $31,760

Note: Some of the specifications may be blank as they were not provided in the input.

Vehicle Market Value Data:
- VIN: JTME6RFV0ND522512
- Success: true
- ID: 2022_toyota_rav4_xse-hv
- Vehicle: 2022 Toyota RAV4 XSE HV
- Mean: $39,552.20
- Standard Deviation: $5,372.00
- Count: 39
- Mileage: 20 miles
- Certainty: 98%
- Period: 2021-12-28 to 2023-02-03
- Prices:
  - Average: $39,552.20
  - Below Average: $34,179.43
  - Above Average: $44,924.97
  - Distribution:
    - Group 1: Min $30,791 - Max $31,239 - Count 4
    - Group 2: Min $31,239 - Max $35,559 - Count 4
    - Group 3: Min $35,559 - Max $37,080 - Count 4
    - Group 4: Min $37,080 - Max $37,514 - Count 4
    - Group 5: Min $37,514 - Max $40,278 - Count 4
    - Group 6: Min $40,278 - Max $40,792 - Count 4
    - Group 7: Min $40,792 - Max $41,743 - Count 4
    - Group 8: Min $41,743 - Max $43,914 - Count 4
    - Group 9: Min $43,914 - Max $46,999 - Count 4
    - Group 10: Min $46,999 - Max $55,656 - Count 3
- Adjustments:
  - Mileage:
    - Average: 20.18 miles
    - Input: 20.18 miles
    - Adjustment: $0.00

Here's the information in a more digestible format:

- VIN: JTME6RFV0ND522512
- Starting Mileage: 15,000 miles
- Mileage per Year: 15,000 miles
- Success: True
- Vehicle: 2022 Toyota Rav4 XSE HV

Yearly Costs:
- Depreciation: $6,720
- Insurance: $2,354
- Fuel: $2,229
- Maintenance: $504
- Repairs: $146
- Fees: $3,483

Total Yearly Cost: $15,435

Over 5 years, the total cost is $54,353.";
    }
    else
    {
      var vinAuditData = await _vinAuditService.GetVinAuditData(vinAuditUserData);
      summary = await _aiService.SummarizeVehicleData(vinAuditData);
    }
    
    var answer = await _aiService.GetAnswer(new GetAnswerRequest
    {
      Data = summary,
      DataInFuture = false,
      Prompt = requestDto.Prompt,
    });

    return answer;
    
    // var queryTopicApi = await _aiService.GetQueryTopicApi(requestDto.Prompt);
    //
    // switch (queryTopicApi)
    // {
    //   case QueryTopicApi.VinAuditSpecifications:
    //     return await _aiService.GetAnswer(new GetAnswerRequest
    //     {
    //       Data = await _vinAuditService.GetMarketValue(vinAuditUserData),
    //       DataInFuture = false,
    //       Prompt = requestDto.Prompt,
    //     });
    //   case QueryTopicApi.VinAuditMarketValue:
    //     return await _aiService.GetAnswer(new GetAnswerRequest
    //     {
    //       Data = await _vinAuditService.GetSpecifications(vinAuditUserData),
    //       DataInFuture = false,
    //       Prompt = requestDto.Prompt,
    //     });
    //   case QueryTopicApi.VinAuditOwnershipCost:
    //     return await _aiService.GetAnswer(new GetAnswerRequest
    //     {
    //       Data = await _vinAuditService.GetOwnershipCost(vinAuditUserData),
    //       DataInFuture = true,
    //       Prompt = requestDto.Prompt,
    //     });
    //   case QueryTopicApi.None:
    //   default:
    //     throw new Exception("No appropriate API found to come up with a satisfying answer to your question.");
    // }
  }
}
