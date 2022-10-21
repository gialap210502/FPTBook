using System.ComponentModel.DataAnnotations;

namespace MvcFPTBook.Models;

public class OrderDetail
{
    public int Id { get; set; }
    public int BookId { get; set; }

    [Required]
    public Book? Book { get; set; }
    public int OrderId { get; set; }

    [Required]
    public Order? Order { get; set; }

    [Required]
    public int Quantity { get; set; }
}
