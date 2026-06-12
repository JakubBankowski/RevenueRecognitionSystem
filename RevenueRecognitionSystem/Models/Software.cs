using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRecognitionSystem.Models;

[Table("Software")]
public class Software
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;
    
    [Required]
    [MaxLength(1000)]
    public string Description { get; set; } = null!;
    
    [Required]
    [MaxLength(50)]
    public string CurrentVersion { get; set; } = null!;
    
    [Required]
    [MaxLength(50)]
    public string Category { get; set; } = null!;
    
    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    public decimal BasePrice { get; set; }
}