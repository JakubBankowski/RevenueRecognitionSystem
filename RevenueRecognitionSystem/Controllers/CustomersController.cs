using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RevenueRecognitionSystem.Data;
using RevenueRecognitionSystem.DTOs.Customer;
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

    [HttpPut("company")]
    public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyRequestDto dto)
    {
        var company = new Company
        {
            Address = dto.address,
            Email = dto.email,
            Krs = dto.krs,
            Phone = dto.phone,
            CompanyName = dto.name,
        };
        
        _context.Companies.Add(company);

        await _context.SaveChangesAsync();
        
        return Ok(company);
    }
    
    [HttpPut("individual")]
    public async Task<IActionResult> CreateIndividual([FromBody] CreateIndividualRequestDto dto)
    {
        var individual = new Individual
        {
            Address = dto.address,
            Email = dto.email,
            Phone = dto.phone,
            FirstName = dto.firstName,
            LastName = dto.lastName,
            Pesel =  dto.pesel
        };
        
        _context.Individuals.Add(individual);

        await _context.SaveChangesAsync();
        
        return Ok(individual);
    }
}