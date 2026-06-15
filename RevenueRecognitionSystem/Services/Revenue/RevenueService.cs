using Microsoft.EntityFrameworkCore;
using RevenueRecognitionSystem.Data;

namespace RevenueRecognitionSystem.Services.Revenue;

public class RevenueService : IRevenueService
{
    private readonly ApplicationDbContext _context;

    public RevenueService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<decimal> GetCurrentRevenueAsync(int? productId)
    {
        var query = _context.Contracts.Where(c => c.IsPaid == true);

        if (productId.HasValue) query = query.Where(c => c.SoftwareId == productId.Value);

        return await query.SumAsync(c => c.TotalPrice);
    }

    public async Task<decimal> GetPredictedRevenueAsync(int? productId)
    {
        DateTime now = DateTime.UtcNow;

        var query = _context.Contracts.Where(c => c.IsPaid == true || (c.IsPaid == false && now <= c.EndDate));

        if (productId.HasValue) query = query.Where(c => c.SoftwareId == productId.Value);

        return await query.SumAsync(c => c.TotalPrice);
    }
}