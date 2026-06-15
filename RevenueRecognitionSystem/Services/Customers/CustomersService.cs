using Microsoft.EntityFrameworkCore;
using RevenueRecognitionSystem.Data;
using RevenueRecognitionSystem.DTOs.Customer;
using RevenueRecognitionSystem.Exceptions;
using RevenueRecognitionSystem.Models;

namespace RevenueRecognitionSystem.Services.Customers;

public class CustomersService : ICustomersService
{
    private readonly ApplicationDbContext _context;

    public CustomersService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Customer> GetCustomerByIdAsync(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null) throw new NotFoundException($"No customer with id: {id} has been found.");
        return customer;
    }

    public async Task<Company> CreateCompanyAsync(CreateCompanyRequestDto dto)
    {
        var companyExists = await _context.Companies.AnyAsync(c => c.Krs == dto.krs);
        if (companyExists) throw new BadRequestException("Company with this KRS already exists.");

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
        return company;
    }

    public async Task<Individual> CreateIndividualAsync(CreateIndividualRequestDto dto)
    {
        var individualExists = await _context.Individuals.AnyAsync(c => c.Pesel == dto.pesel);
        if (individualExists) throw new BadRequestException("Individual with this PESEL already exists.");

        var individual = new Individual
        {
            Address = dto.address,
            Email = dto.email,
            Phone = dto.phone,
            FirstName = dto.firstName,
            LastName = dto.lastName,
            Pesel = dto.pesel
        };

        _context.Individuals.Add(individual);
        await _context.SaveChangesAsync();
        return individual;
    }

    public async Task<Company> UpdateCompanyAsync(int id, UpdateCompanyRequestDto dto)
    {
        var company = await _context.Companies.FindAsync(id);
        if (company == null) throw new NotFoundException($"No company with id: {id} has been found.");

        company.CompanyName = dto.companyName;
        company.Address = dto.address;
        company.Email = dto.email;
        company.Phone = dto.phoneNumber;

        await _context.SaveChangesAsync();
        return company;
    }

    public async Task<Individual> UpdateIndividualAsync(int id, UpdateIndividualRequestDto dto)
    {
        var individual = await _context.Individuals.FindAsync(id);
        if (individual == null) throw new NotFoundException($"No individual with id: {id} hasbeen found.");

        individual.FirstName = dto.firstName;
        individual.LastName = dto.lastName;
        individual.Address = dto.address;
        individual.Email = dto.email;
        individual.Phone = dto.phoneNumber;

        await _context.SaveChangesAsync();
        return individual;
    }

    public async Task DeleteCustomerAsync(int id)
    {
        var customer = await _context.Customers.FindAsync(id);
        if (customer == null) throw new NotFoundException("Customer not found.");

        if (customer is Company)
        {
            throw new BadRequestException("Company data cannot be deleted");
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
    }
}