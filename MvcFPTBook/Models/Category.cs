using System.ComponentModel.DataAnnotations;
namespace MvcFPTBook.Models;

public class Category{
    [Required]
    public int Id { get; set; }
    [Required]
    [StringLength(255)]
    public string Name { get; set; }
    [Required]
    public string Status { get; set; }
    public ICollection<Book> Books { get; set; }
}