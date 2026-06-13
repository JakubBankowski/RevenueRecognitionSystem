using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RevenueRecognitionSystem.Data;
using RevenueRecognitionSystem.DTOs.Contract;
using RevenueRecognitionSystem.Models;

namespace RevenueRecognitionSystem.Controllers;

[Authorize]
[ApiController]
[Route("api/contracts")]
public class ContractsController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    public ContractsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> CreateContract([FromBody] CreateContractRequestDto dto)
    {
        int days = (dto.endDate - dto.startDate).Days;
        if (days < 3 || days > 30)
        {
            return BadRequest(new { Message = "Contract duration must be between 3 and 30 days." });
        }

        bool hasActiveContract = await _context.Contracts.AnyAsync(c =>
            c.CustomerId == dto.customerId &&
            c.SoftwareId == dto.softwareId &&
            c.IsPaid == true);

        if (hasActiveContract)
            return BadRequest(new { Message = "Customer already has an active contract for this software." });

        var software = await _context.Softwares.FindAsync(dto.softwareId);
        var customer = await _context.Customers.Include(c => c.Contracts).FirstOrDefaultAsync(c => c.Id == dto.customerId);

        if (software == null || customer == null) return NotFound(new { Message = "Software or Customer not found." });

        decimal basePrice = software.BasePrice + (dto.additionalSupportYears * 1000m);

        DateTime now = DateTime.UtcNow;
        var activeDiscounts = await _context.Discounts
            .Where(d => now >= d.From && now <= d.To)
            .ToListAsync();

        decimal highestDiscount = activeDiscounts.Any() ? activeDiscounts.Max(d => d.Value) : 0m;

        bool isReturning = customer.Contracts.Any(c => c.IsPaid);
        if (isReturning) highestDiscount += 5.00m;

        decimal finalPrice = basePrice * (1.00m - (highestDiscount / 100m));

        var contract = new Contract
        {
            CustomerId = dto.customerId,
            SoftwareId = dto.softwareId,
            SoftwareVersion = software.CurrentVersion,
            StartDate = dto.startDate,
            EndDate = dto.endDate,
            TotalPrice = finalPrice,
            AdditionalSupportYears = dto.additionalSupportYears,
            IsPaid = false
        };

        _context.Contracts.Add(contract);
        await _context.SaveChangesAsync();
        
        return Ok(contract);
    }
}