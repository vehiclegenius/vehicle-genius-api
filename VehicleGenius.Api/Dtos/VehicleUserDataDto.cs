namespace VehicleGenius.Api.Dtos;

public class VehicleUserDataDto
{
  public Decimal InsuranceRate { get; set; }
  public string InsuranceProvider { get; set; }
  public DateOnly InsuranceRenewalDate { get; set; }
  public Decimal FinancingInterestRate { get; set; }
  public DateOnly FinancingTermEnd { get; set; }
  public string PreviousMaintenanceData { get; set; }
}
