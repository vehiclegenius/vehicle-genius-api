namespace VehicleGenius.Api.Dtos;

public class SummaryTemplateValidationResultDto
{
  public bool IsValid { get; set; }
  public string ErrorMessage { get; set; }
  public string Preview { get; set; }
}
