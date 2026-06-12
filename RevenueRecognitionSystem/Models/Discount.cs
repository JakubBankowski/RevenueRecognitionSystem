using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRecognitionSystem.Models;

[Table("Discounts")]
public class Discount
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = null!;
    
    [Required]
    [Column(TypeName = "decimal(5,2)")]
    public decimal Value { get; set; }
    
    [Required]
    public DateTime From { get; set; }
    
    [Required]
    public DateTime To { get; set; }
}