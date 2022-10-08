using System.ComponentModel.DataAnnotations;
namespace MvcFPTBook.Models;

public class Publisher{
    [Required]
    public int Id { get; set; }
    [Required]
    [StringLength(255)]
    public string? Name { get; set; }
    [Required]
    [StringLength(255)]
    public string? Address { get; set; }
    [Required]
    [StringLength(10)]
    public string? Phone { get; set; }
    public ICollection<Book>? Books { get; set; }
}