using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Validators;

namespace MvcStore.Models;

public class OrderedProducts
{
    [Key]
    [Required]
    [Display(Name = "Order ID")]
    public int OrderID { get; set; }
    public Order Order { get; set; }

    [Key]
    [Required]
    [Display(Name = "Product ID")]
    public int ProductID { get; set; }
    public Product Product { get; set; }

    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public int Quantity { get; set; }
}
