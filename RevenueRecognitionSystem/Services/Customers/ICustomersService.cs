using RevenueRecognitionSystem.DTOs.Customer;
using RevenueRecognitionSystem.Models;

namespace RevenueRecognitionSystem.Services.Customers;

public interface ICustomersService
{
    Task<Customer> GetCustomerByIdAsync(int id);
    Task<Company> CreateCompanyAsync(CreateCompanyRequestDto dto);
    Task<Individual> CreateIndividualAsync(CreateIndividualRequestDto dto);
    Task<Company> UpdateCompanyAsync(int id, UpdateCompanyRequestDto dto);
    Task<Individual> UpdateIndividualAsync(int id, UpdateIndividualRequestDto dto);
    Task DeleteCustomerAsync(int id);
}