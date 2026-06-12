using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRecognitionSystem.Models;

[Table("Contracts")]
public class Contract
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int CustomerId { get; set; }

    [ForeignKey(nameof(CustomerId))]
    public Customer Customer { get; set; } = null!;
    
    [Required]
    public int SoftwareId { get; set; }

    [ForeignKey(nameof(SoftwareId))]
    public Software Software { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string SoftwareVersion { get; set; } = null!;
    
    [Required]
    public DateTime StartDate { get; set; }
    
    [Required]
    public DateTime EndDate { get; set; }

    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal TotalPrice { get; set; }
    
    [Range(0, 3)]
    public int AdditionalSupportYears { get; set; }
    
    public bool IsPaid { get; set; }
    
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}