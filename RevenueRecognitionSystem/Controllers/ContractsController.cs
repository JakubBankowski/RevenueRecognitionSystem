using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RevenueRecognitionSystem.Data;
using RevenueRecognitionSystem.DTOs.Contract;
using RevenueRecognitionSystem.Exceptions;
using RevenueRecognitionSystem.Models;
using RevenueRecognitionSystem.Services.Contracts;

namespace RevenueRecognitionSystem.Controllers;

[Authorize]
[ApiController]
[Route("api/contracts")]
public class ContractsController : ControllerBase
{
    private readonly IContractsService _contractsService;

    public ContractsController(IContractsService contractsService)
    {
        _contractsService = contractsService;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateContract([FromBody] CreateContractRequestDto dto)
    {
        try {
            var contract = await _contractsService.CreateContractAsync(dto);
            return Ok(contract);
        } 
        catch (BadRequestException ex) { return BadRequest(ex.Message); }
        catch (NotFoundException ex) { return NotFound(ex.Message); }
    }
}