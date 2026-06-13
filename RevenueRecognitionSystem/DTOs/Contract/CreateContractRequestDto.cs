namespace RevenueRecognitionSystem.DTOs.Contract;

public class CreateContractRequestDto
{
    public int customerId { get; set; }
    public int softwareId {get; set;}
    public DateTime startDate { get; set; }
    public DateTime endDate { get; set; }
    public int additionalSupportYears { get; set; }
}