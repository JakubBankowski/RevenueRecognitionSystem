using Microsoft.EntityFrameworkCore;
using RevenueRecognitionSystem.Data;
using RevenueRecognitionSystem.DTOs.Contract;
using RevenueRecognitionSystem.Exceptions;
using RevenueRecognitionSystem.Models;

namespace RevenueRecognitionSystem.Services.Contracts;

public class ContractsService : IContractsService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    
    public ContractsService(ApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    
    public async Task<Contract> CreateContractAsync(CreateContractRequestDto dto)
    {
        int days = (dto.endDate - dto.startDate).Days;
        if (days < 3 || days > 30)
        {
            throw new BadRequestException("Contract duration must be between 3 and 30 days.");
        }

        bool hasActiveContract = await _context.Contracts.AnyAsync(c =>
            c.CustomerId == dto.customerId &&
            c.SoftwareId == dto.softwareId &&
            c.IsPaid == true);

        if (hasActiveContract)
        {
            throw new BadRequestException("Customer already has an active contract for this software.");
        }

        var software = await _context.Softwares.FindAsync(dto.softwareId);
        var customer = await _context.Customers.Include(c => c.Contracts).FirstOrDefaultAsync(c => c.Id == dto.customerId);

        if (software == null || customer == null) 
        {
            throw new NotFoundException("Software or Customer not found.");
        }

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

        return contract;
    }
}