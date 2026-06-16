using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RevenueRecognitionSystem.Models;

namespace RevenueRecognitionSystem.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}

    public DbSet<Customer> Customers { get; set; } = null!;
    public DbSet<Individual> Individuals { get; set; } = null!;
    public DbSet<Company> Companies { get; set; } = null!;
    
    public DbSet<Software> Softwares { get; set; } = null!;
    public DbSet<Discount> Discounts { get; set; } = null!;
    public DbSet<Contract> Contracts { get; set; } = null!;
    public DbSet<Payment> Payments { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Individual>().HasIndex(i => i.Pesel).IsUnique();
        modelBuilder.Entity<Company>().HasIndex(c => c.Krs).IsUnique();

        modelBuilder.Entity<Customer>().HasQueryFilter(i => !i.IsDeleted);

        var individuals = new[]
        {
            new Individual
            {
                Id = 1, FirstName = "John", LastName = "Doe", Pesel = "12345678912", Address = "Serkowa 2",
                Email = "serek@pl", Phone = "111222333"
            }
        };
        var companies = new[]
        {
            new Company
            {
                Id = 2, Address = "Serkowa 5", Email = "serek@com", Phone = "444222333", CompanyName = "SerekCo",
                Krs = "1234567890"
            }
        };
        var softwares = new[]
        {
            new Software
            {
                Id = 1, Name = "SerekOS", Description = "OS serkowe takie o ", CurrentVersion = "1.1",
                Category = "serowa", BasePrice = 123456
            }
        };
        var discounts = new[]
        {
            new Discount
            {
                Id = 1, Name = "Wiosenna", Value = 50.00m, From = new DateTime(2025, 6, 1, 7, 47, 0),
                To = new DateTime(2026, 6, 1, 7, 47, 0)
            }
        };
        var contracts = new[]
        {
            new Contract
            {
                Id = 1, CustomerId = 1, SoftwareId = 1, SoftwareVersion = "1.1",
                StartDate = new DateTime(2025, 6, 1, 7, 47, 0), EndDate = new DateTime(2026, 6, 1, 7, 47, 0),
                TotalPrice = 123456, AdditionalSupportYears = 0, IsPaid = true
            }
        };
        var payments = new[]
        {
            new Payment
            {
                Id = 1, ContractId = 1, Amount = 123456, DateReceived = new DateTime(2026, 6, 1, 7, 47, 0),
            }
        };
        
        var admin = new User
        {
            Id = 1,
            Username = "admin",
            Role = "Admin",
            PasswordHash = "AQAAAAIAAYagAAAAEAHs0S3qs6Y77j5fRU9X+akhT0z421ZyRiDfTrgekvo2hmcx9O7AgYEW/3jfulJatw==",
        };
        
        var users = new[]
        {
            admin
        };
        
        modelBuilder.Entity<Contract>().HasData(contracts);
        modelBuilder.Entity<Company>().HasData(companies);
        modelBuilder.Entity<Discount>().HasData(discounts);
        modelBuilder.Entity<Payment>().HasData(payments);
        modelBuilder.Entity<Software>().HasData(softwares);
        modelBuilder.Entity<User>().HasData(users);
        modelBuilder.Entity<Individual>().HasData(individuals);
    }
}