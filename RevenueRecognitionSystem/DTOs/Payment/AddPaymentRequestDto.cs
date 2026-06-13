namespace RevenueRecognitionSystem.DTOs.Payment;

public class AddPaymentRequestDto
{
    public int contractId { get; set; }
    public decimal amount { get; set; }
}