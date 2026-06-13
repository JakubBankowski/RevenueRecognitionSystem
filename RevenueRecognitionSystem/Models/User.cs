using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RevenueRecognitionSystem.Models;

[Table("Users")]
public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [MaxLength(100)]
    public string Username { get; set; } = null!;
    
    [Required]
    [MaxLength(500)]
    public string PasswordHash { get; set; } = null!;
    
    [Required]
    [MaxLength(50)]
    public string Role { get; set; } = null!;
}