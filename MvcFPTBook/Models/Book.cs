using System.ComponentModel.DataAnnotations;

namespace MvcFPTBook.Models;

public class Book
{
    [Required]
    public int Id { get; set; }

    [Required]
    [StringLength(255)]
    public string? Name { get; set; }

    [Required]
    [DataType(DataType.ImageUrl)]
    public string Poster { get; set; }

    [DataType(DataType.Date)]
    public DateTime publiccationDate { get; set; }

    [Required]
    public double Price { get; set; }

    [Required]
    public int AuthorID { get; set; }

    [Required]
    public Author? Author { get; set; }

    [Required]
    public int CategoryID { get; set; }

    [Required]
    public Category? Category { get; set; }

    [Required]
    public int PublisherID { get; set; }

    [Required]
    public Publisher? Publishers { get; set; }

    public ICollection<OrderDetail>? OrderDetails { get; set; }
}
