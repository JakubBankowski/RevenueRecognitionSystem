using RevenueRecognitionSystem.DTOs.Auth;
using RevenueRecognitionSystem.DTOs.Contract;
using RevenueRecognitionSystem.Models;

namespace RevenueRecognitionSystem.Services.Contracts;

public interface IContractsService
{
    Task<Contract> CreateContractAsync(CreateContractRequestDto dto);
}