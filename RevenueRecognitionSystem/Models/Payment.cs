using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRecognitionSystem.Models;

[Table("Payments")]
public class Payment
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public int ContractId { get; set; }

    [ForeignKey(nameof(ContractId))]
    public Contract Contract { get; set; } = null!;
    
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }
    
    [Required]
    public DateTime DateReceived { get; set; }
}