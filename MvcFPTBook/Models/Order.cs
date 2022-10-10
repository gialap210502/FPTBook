using System.ComponentModel.DataAnnotations;
namespace MvcFPTBook.Models;

public class Order
{
    public int Id { get; set; }
    public int State { get; set; }
    [DataType(DataType.Date)]
    public DateTime OrderTime { get; set; }
    public decimal Total { get; set; }
    public User? User { get; set; }
    public ICollection<OrderDetail>? OrderDetail { get; set; }






}