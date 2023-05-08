using Microsoft.AspNetCore.Mvc;
using MvcStore.Models;
using MvcStore.Data;
using Microsoft.EntityFrameworkCore;

namespace MvcStore.Controllers;

public class OrderedProductsController : Controller
{
    private readonly MvcStoreContext _context;

    public OrderedProductsController(MvcStoreContext context)
    {
        _context = context;
    }

    // GET: OrderedProducts/Index
    public async Task<IActionResult> Index(string productName)
    {
        var orderedProducts = await _context.OrderedProducts.Select(x => x).Include(x => x.Order).Include(x => x.Product).ToListAsync();
        if (!String.IsNullOrEmpty(productName))
        {
            orderedProducts = orderedProducts.Where(x => x.Product.Name.Contains(productName)).ToList();
        }

        var products = await _context.Product.Select(x => x).ToListAsync();


        var viewModel = new OrderedProductsViewModel
        {
            OrderedProducts = orderedProducts,
            Products = products
        };
        
        return View(viewModel);
    }

}
