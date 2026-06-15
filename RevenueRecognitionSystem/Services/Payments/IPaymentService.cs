using RevenueRecognitionSystem.DTOs.Payment;

namespace RevenueRecognitionSystem.Services.Payments;

public interface IPaymentService
{
    Task<(string Message, bool ContractFullyPaid)> IssuePaymentAsync(AddPaymentRequestDto dto);
}