using Microsoft.EntityFrameworkCore;
using RevenueRecognitionSystem.Data;
using RevenueRecognitionSystem.DTOs.Payment;
using RevenueRecognitionSystem.Exceptions;
using RevenueRecognitionSystem.Models;

namespace RevenueRecognitionSystem.Services.Payments;

public class PaymentService : IPaymentService
{
    private readonly ApplicationDbContext _context;

    public PaymentService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<(string Message, bool ContractFullyPaid)> IssuePaymentAsync(AddPaymentRequestDto dto)
    {
        var contract = await _context.Contracts
            .Include(c => c.Payments)
            .FirstOrDefaultAsync(c => c.Id == dto.contractId);

        if (contract == null) throw new NotFoundException("Contract not found.");

        if (DateTime.UtcNow > contract.EndDate)
        {
            throw new BadRequestException("Offer expired. Payments must be refunded");
        }

        decimal currentPaidSum = contract.Payments.Sum(p => p.Amount);
        decimal remainingToPay = contract.TotalPrice - currentPaidSum;

        if (dto.amount > remainingToPay)
        {
            throw new BadRequestException($"Paid too much. Remaining amount is {remainingToPay} PLN.");
        }

        var payment = new Payment
        {
            ContractId = dto.contractId,
            Amount = dto.amount,
            DateReceived = DateTime.UtcNow
        };

        _context.Payments.Add(payment);

        if (currentPaidSum + dto.amount == contract.TotalPrice)
        {
            contract.IsPaid = true;
        }

        await _context.SaveChangesAsync();
        return ("Payment logged successfully.", contract.IsPaid);
    }
}