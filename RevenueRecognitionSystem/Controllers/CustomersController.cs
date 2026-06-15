using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileSystemGlobbing;
using RevenueRecognitionSystem.Data;
using RevenueRecognitionSystem.DTOs.Customer;
using RevenueRecognitionSystem.Exceptions;
using RevenueRecognitionSystem.Models;
using RevenueRecognitionSystem.Services.Customers;

namespace RevenueRecognitionSystem.Controllers;

[Authorize]
[ApiController]
[Route("api/customers")]
public class CustomersController : ControllerBase
{
    private readonly ICustomersService _customersService;

    public CustomersController(ICustomersService customersService)
    {
        _customersService = customersService;
    } 

    [HttpGet("{id}")]
    public async Task<IActionResult> GetCustomer(int id)
    {
        try { return Ok(await _customersService.GetCustomerByIdAsync(id)); }
        catch (NotFoundException ex) { return NotFound(ex.Message); }
    }

    [HttpPost("company")]
    public async Task<IActionResult> CreateCompany([FromBody] CreateCompanyRequestDto dto)
    {
        try { return Ok(await _customersService.CreateCompanyAsync(dto)); }
        catch (BadRequestException ex) { return BadRequest(ex.Message); }
    }

    [HttpPost("individual")]
    public async Task<IActionResult> CreateIndividual([FromBody] CreateIndividualRequestDto dto)
    {
        try { return Ok(await _customersService.CreateIndividualAsync(dto)); }
        catch (BadRequestException ex) { return BadRequest(ex.Message); }
    }

    [HttpPut("company/{id}")]
    public async Task<IActionResult> UpdateCompany(int id, [FromBody] UpdateCompanyRequestDto dto)
    {
        try { return Ok(await _customersService.UpdateCompanyAsync(id, dto)); }
        catch (NotFoundException ex) { return NotFound(ex.Message); }
    }

    [HttpPut("individual/{id}")]
    public async Task<IActionResult> UpdateIndividual(int id, [FromBody] UpdateIndividualRequestDto dto)
    {
        try { return Ok(await _customersService.UpdateIndividualAsync(id, dto)); }
        catch (NotFoundException ex) { return NotFound(ex.Message); }
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        try {
            await _customersService.DeleteCustomerAsync(id);
            return NoContent();
        }
        catch (NotFoundException ex) { return NotFound(ex.Message); }
        catch (BadRequestException ex) { return BadRequest(ex.Message); }
    }
}