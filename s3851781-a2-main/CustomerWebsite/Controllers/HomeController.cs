﻿using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using CustomerWebsite.Models;
using Microsoft.AspNetCore.Authorization;

namespace CustomerWebsite.Controllers;

[AllowAnonymous]
public class HomeController : Controller
{
    public IActionResult Index() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
