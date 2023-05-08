namespace MvcStore.Models;

public class OrderedProductsViewModel
{
    public IEnumerable<OrderedProducts> OrderedProducts { get; set; }
    public IEnumerable<Product> Products { get; set; }
}
