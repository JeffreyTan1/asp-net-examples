using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WebApi.Validators;

namespace MvcStore.Models;

public class Product
{
    [Key]
    [Required]
    [Display(Name = "Product ID")]
    public int ProductID { get; set; }

    [Required]
    [ProductNameAttribute]
    [StringLength(50)]
    public string Name { get; set; }

    [Required]
    [Column(TypeName = "decimal")]
    [DataType(DataType.Currency)]
    public decimal Price { get; set; }

}
