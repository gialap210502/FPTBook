using System.ComponentModel.DataAnnotations;
namespace MvcFPTBook.Models;

public class User{
    [Required]
    public int Id { get; set; }
     [Required]
    [StringLength(255)]
    public string? Name { get; set; }
     [Required]
    [StringLength(255)]
    public string? UserName { get; set; }
    [Required]
    [StringLength(255)]
    public string? Password { get; set; }
    [Required]
    [StringLength(255)]
    public string? Email { get; set; }
    [Required]
    [StringLength(10)]
    public string? Phone { get; set; }
    [Required]
    public DateTime DateOfBirth { get; set; }
    [Required]
    public string? roles { get; set; }
    public ICollection<Order>? Orders { get; set;}
}