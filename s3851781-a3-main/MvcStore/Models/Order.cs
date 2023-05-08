using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Validators;

namespace MvcStore.Models;

public class Order
{
    [Key]
    [Required]
    [Display(Name = "Order ID")]
    public int OrderID { get; set; }

    [Required]
    [DateTime]
    [DataType(DataType.Date)]
    [Display(Name = "Order Date")]
    public DateTime OrderDate { get; set; }

    [Required]
    [CustomerNameAttribute]
    [StringLength(50)]
    public string CustomerName { get; set; }

    [Required]
    [StringLength(200)]
    public string DeliveryAddress { get; set; }

    [DateTime]
    [DataType(DataType.Date)]
    public DateTime? DeliveredDate { get; set; }

    public ICollection<OrderedProducts> OrderedProducts { get; set; }
}
