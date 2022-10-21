using System.ComponentModel.DataAnnotations;
using MvcFPTBook.Areas.Identity.Data;

namespace MvcFPTBook.Models;

public class Order
{
    public int Id { get; set; }
    public int State { get; set; }

    [DataType(DataType.Date)]
    public DateTime OrderTime { get; set; }
    public decimal Total { get; set; }
    public string BookUserId { get; set; }

    public BookUser? BookUser { get; set; }

    public ICollection<OrderDetail>? OrderDetail { get; set; }
}
