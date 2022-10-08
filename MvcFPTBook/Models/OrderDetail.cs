using System.ComponentModel.DataAnnotations;

namespace MvcFPTBook.Models;

public class OrderDetail{
    [Required]
    public int Id { get; set; }
    [Required]
    public Book? Book { get; set; }
    [Required]
    public Order? Order { get; set; }
    [Required]
    public int Quantity { get; set; }

    

}