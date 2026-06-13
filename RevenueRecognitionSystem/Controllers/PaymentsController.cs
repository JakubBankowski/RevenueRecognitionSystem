using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RevenueRecognitionSystem.Data;
using RevenueRecognitionSystem.DTOs.Payment;
using RevenueRecognitionSystem.Models;

namespace RevenueRecognitionSystem.Controllers;

[Authorize]
[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    
    public PaymentsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> IssuePayment([FromBody] AddPaymentRequestDto dto)
    {
        var contract = await _context.Contracts
            .Include(c => c.Payments)
            .FirstOrDefaultAsync(c => c.Id == dto.contractId);
        
        if (contract == null) return NotFound();

        if (DateTime.UtcNow > contract.EndDate)
        {
            return BadRequest(new { Message = "Offer expired. Payments must be refunded" });
        }

        decimal currentPaidSum = contract.Payments.Sum(p => p.Amount);
        decimal remainingToPay = contract.TotalPrice - currentPaidSum;

        if (dto.amount > remainingToPay)
        {
            return BadRequest(new { Message = $"Paid too much. Remaining amount is {remainingToPay} PLN." });
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
        return Ok(new { Message = "Payment logged successfully.", ContractFullyPaid = contract.IsPaid });
    }
}