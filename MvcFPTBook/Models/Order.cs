using System.ComponentModel.DataAnnotations;
namespace MvcFPTBook.Models;

public class Order{
    public int Id { get; set; }
    public bool Status { get; set; }
    public DateTime Created_at { get; set; }
    public DateTime Updated_at { get; set; }
    public User? User { get; set; }
    public ICollection<OrderDetail>? OrderDetail { get; set; }
}