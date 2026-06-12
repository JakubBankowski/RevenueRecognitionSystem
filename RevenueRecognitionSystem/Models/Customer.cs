using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRecognitionSystem.Models;

[Table("Customers")]
public abstract class Customer
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string Address { get; set; } = null!;
    
    [Required]
    [MaxLength(50)]
    public string Email { get; set; } = null!;
    
    [Required]
    [MaxLength(9)]
    [MinLength(9)]
    public string Phone { get; set; } = null!;
    
    public bool IsReturningClient { get; set; }
    public bool IsDeleted { get; set; } = false;
    
    public ICollection<Contract> Contracts { get; set; } = new List<Contract>();
}

public class Individual : Customer
{
    [Required] 
    [MaxLength(50)]
    public string FirstName { get; set; } = null!;
    
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; } = null!;
    
    [Required]
    [MaxLength(11)]
    [MinLength(11)]
    public string Pesel { get; set; } = null!;
    
}

public class Company : Customer
{
    [Required] 
    [MaxLength(100)]
    public string CompanyName { get; set; } = null!;
    
    [Required]
    [MaxLength(10)]
    [MinLength(10)]
    public string Krs { get; set; } = null!;
}