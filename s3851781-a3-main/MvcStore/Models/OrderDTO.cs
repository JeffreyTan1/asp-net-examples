using System.ComponentModel.DataAnnotations;

namespace MvcStore.Models;

public class OrderDTO : Order
{
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public int Quantity { get; set; }
    
}
