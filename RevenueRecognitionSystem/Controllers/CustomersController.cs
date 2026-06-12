using Microsoft.AspNetCore.Mvc;
using RevenueRecognitionSystem.Data;
using RevenueRecognitionSystem.Models;

namespace RevenueRecognitionSystem.Controllers;

[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly ApplicationDbContext _context;
    
    public CustomersController(ApplicationDbContext context)
    {
        _context = context;
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomer(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        return Ok(customer);
    }
}