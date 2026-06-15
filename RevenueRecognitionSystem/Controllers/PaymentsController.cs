using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RevenueRecognitionSystem.Data;
using RevenueRecognitionSystem.DTOs.Payment;
using RevenueRecognitionSystem.Exceptions;
using RevenueRecognitionSystem.Models;
using RevenueRecognitionSystem.Services.Payments;

namespace RevenueRecognitionSystem.Controllers;

[Authorize]
[ApiController]
[Route("api/payments")]
public class PaymentsController : ControllerBase
{
    private readonly IPaymentService _paymentsService;

    public PaymentsController(IPaymentService paymentsService)
    { 
        _paymentsService = paymentsService;
    } 

    [HttpPost]
    public async Task<IActionResult> IssuePayment([FromBody] AddPaymentRequestDto dto)
    {
        try {
            var result = await _paymentsService.IssuePaymentAsync(dto);
            return Ok(new { Message = result.Message, ContractFullyPaid = result.ContractFullyPaid });
        }
        catch (NotFoundException ex) { return NotFound(ex.Message); }
        catch (BadRequestException ex) { return BadRequest(ex.Message); }
    }
}