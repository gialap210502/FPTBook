using System.ComponentModel.DataAnnotations;
namespace MvcFPTBook.Models;

public class Author{
    [Required]
    public int Id { get; set; }
    [Required]
    [StringLength(255)]
    public string? Name { get; set; }
    public ICollection<Book>? Books { get; set; }
}