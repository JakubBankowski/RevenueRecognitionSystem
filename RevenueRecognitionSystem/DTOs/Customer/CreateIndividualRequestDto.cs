namespace RevenueRecognitionSystem.DTOs.Customer;

public class CreateIndividualRequestDto
{ 
    public string firstName { get; set; }
    public string lastName { get; set; }
    public string email { get; set; }
    public string phone { get; set; }
    public string address { get; set; }
    public string pesel { get; set; }
}