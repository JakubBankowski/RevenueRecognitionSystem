using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RevenueRecognitionSystem.Data;

namespace RevenueRecognitionSystem.Controllers;

[Authorize]
[ApiController]
[Route("api/revenue")]
public class RevenueController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    
    public RevenueController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentRevenue([FromQuery] int? productId)
    {
        var query = _context.Contracts.Where(c => c.IsPaid == true);

        if (productId.HasValue) query = query.Where(c => c.SoftwareId == productId.Value);

        decimal total = await query.SumAsync(c => c.TotalPrice);
        return Ok(new{Revenue = total, Type = "Current Recognized Revenue"});
    }

    public async Task<IActionResult> GetPredictedRevenue([FromQuery] int? productId)
    {
        DateTime now = DateTime.UtcNow;

        var query = _context.Contracts.Where(c => c.IsPaid == true || (c.IsPaid == false && now <= c.EndDate));

        if (productId.HasValue)
        {
            query = query.Where(c=> c.SoftwareId == productId.Value);
        }

        decimal predictedRevenue = await query.SumAsync(c => c.TotalPrice);
        return Ok(new {Revenue = predictedRevenue, Type = "Predicted Potential Revenue"});
    }
}