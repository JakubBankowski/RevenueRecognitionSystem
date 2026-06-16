using Microsoft.EntityFrameworkCore;
using RevenueRecognitionSystem.Data;
using RevenueRecognitionSystem.DTOs.Contract;
using RevenueRecognitionSystem.Exceptions;
using RevenueRecognitionSystem.Models;

namespace RevenueRecognitionSystem.Services.Contracts;

public class ContractsService : IContractsService
{
    private readonly ApplicationDbContext _context;
    
    public ContractsService(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<Contract> CreateContractAsync(CreateContractRequestDto dto)
    {
        int days = (dto.endDate - dto.startDate).Days;
        if (days < 3 || days > 30)
        {
            throw new BadRequestException("Contract duration must be between 3 and 30 days.");
        }

        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            bool hasActiveContract = await _context.Contracts.AnyAsync(c =>
                c.CustomerId == dto.customerId &&
                c.SoftwareId == dto.softwareId &&
                !c.IsPaid);

            if (hasActiveContract)
            {
                throw new BadRequestException("Customer already has an active contract for this software.");
            }

            var software = await _context.Softwares.FindAsync(dto.softwareId);
            if (software == null) throw new NotFoundException("Software not found.");

            bool customerExists = await _context.Customers.AnyAsync(c => c.Id == dto.customerId);
            if (!customerExists) throw new NotFoundException("Customer not found.");

            decimal basePrice = software.BasePrice + (dto.additionalSupportYears * 1000m);
            DateTime now = DateTime.UtcNow;

            decimal highestDiscount = await _context.Discounts
                .Where(d => now >= d.From && now <= d.To)
                .Select(d => (decimal?)d.Value)
                .MaxAsync() ?? 0m;

            bool isReturning = await _context.Contracts.AnyAsync(c => c.CustomerId == dto.customerId && c.IsPaid);
            if (isReturning) 
            {
                highestDiscount += 5.00m; 
            }

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
            
            await transaction.CommitAsync();
            return contract;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
}