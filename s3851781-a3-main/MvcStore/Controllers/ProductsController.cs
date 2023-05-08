using Microsoft.AspNetCore.Mvc;
using MvcStore.Models;
using MvcStore.Data;
using Microsoft.EntityFrameworkCore;

namespace MvcStore.Controllers;

public class ProductsController : Controller
{
    private readonly MvcStoreContext _context;

    public ProductsController(MvcStoreContext context)
    {
        _context = context;
    }

    // GET: Products/Index
    public async Task<IActionResult> Index()
    {
        var products = await _context.Product.Select(x => x).ToListAsync();
        return View(products);
    }

    // GET: Products/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Products/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Product product)
    {
        if(ModelState.IsValid)
        {
            _context.Product.Add(product);
            _context.SaveChanges();
            return RedirectToAction("Index");                
        }

        return View(product);
    }
        
}
