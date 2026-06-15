using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RevenueRecognitionSystem.Data;
using RevenueRecognitionSystem.Services.Revenue;

namespace RevenueRecognitionSystem.Controllers;

[Authorize]
[ApiController]
[Route("api/revenue")]
public class RevenueController : ControllerBase
{
    private readonly IRevenueService _revenueService;

    public RevenueController(IRevenueService revenueService)
    {
        _revenueService = revenueService;
    } 

    [HttpGet("current")]
    public async Task<IActionResult> GetCurrentRevenue([FromQuery] int? productId)
    {
        var total = await _revenueService.GetCurrentRevenueAsync(productId);
        return Ok(new { Revenue = total, Type = "Current Recognized Revenue" });
    }

    [HttpGet("predicted")]
    public async Task<IActionResult> GetPredictedRevenue([FromQuery] int? productId)
    {
        var predicted = await _revenueService.GetPredictedRevenueAsync(productId);
        return Ok(new { Revenue = predicted, Type = "Predicted Potential Revenue" });
    }
}