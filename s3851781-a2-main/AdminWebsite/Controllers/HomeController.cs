using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AdminWebsite.Models;

namespace AdminWebsite.Controllers;

public class HomeController : Controller
{
    // ReSharper disable once NotAccessedField.Local
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        return View();
    }
}
