using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing;
using RevenueRecognitionSystem.Data;
using RevenueRecognitionSystem.DTOs.Customer;
using RevenueRecognitionSystem.Models;

namespace RevenueRecognitionSystem.Controllers;

[Authorize]
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
        if (customer == null) return NotFound($"No customer with id: {id} has been found.");
        return Ok(customer);
    }

    [HttpPost("company")]
    public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyRequestDto dto)
    {
        var companyExists = await _context.Companies.AnyAsync(c => c.Krs == dto.krs);
        if (companyExists) return BadRequest("Company with this KRS already exists.");
        
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
    
    [HttpPost("individual")]
    public async Task<IActionResult> CreateIndividual([FromBody] CreateIndividualRequestDto dto)
    {
        var individualExists = await _context.Individuals.AnyAsync(c => c.Pesel == dto.pesel);
        if (individualExists) return BadRequest("Individual with this PESEL already exists.");
        
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

    [HttpPut("company/{id}")]
    public async Task<IActionResult> UpdateCompany(int id, [FromBody] UpdateCompanyRequestDto dto)
    {
        var company = await _context.Companies.FindAsync(id);
        if (company == null) return NotFound($"No company with id: {id} has been found.");
        
        company.CompanyName = dto.companyName;
        company.Address = dto.address;
        company.Email = dto.email;
        company.Phone = dto.phoneNumber;
        
        await _context.SaveChangesAsync();
        return Ok(company);
    }
    
    [HttpPut("individual/{id}")]
    public async Task<IActionResult> UpdateIndividual(int id, [FromBody] UpdateIndividualRequestDto dto)
    {
        var individual = await _context.Individuals.FindAsync(id);
        if (individual == null) return NotFound($"No individual with id: {id} has been found.");
        
        individual.FirstName = dto.firstName;
        individual.LastName = dto.lastName;
        individual.Address = dto.address;
        individual.Email = dto.email;
        individual.Phone = dto.phoneNumber;

        await _context.SaveChangesAsync();
        return Ok(individual);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null) return NotFound(new { Message = "Customer not found. " });
        
        if (customer is Company)
        {
            return BadRequest(new { Message = "Company data cannot be deleted." });
        }

        if (customer is Individual individual)
        {
            individual.IsDeleted = true;

            individual.FirstName = "DELETED";
            individual.LastName = "DELETED";
            individual.Email = "DELETED";
            individual.Phone = "000000000";
        }

        await _context.SaveChangesAsync();
        return NoContent();
    }
}