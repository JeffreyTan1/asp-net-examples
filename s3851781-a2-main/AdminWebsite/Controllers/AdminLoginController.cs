using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using AdminWebsite.Models;

namespace AdminWebsite.Controllers;

[AllowAnonymous]
public class AdminLoginController : Controller
{
    private string loggedInState => HttpContext.Session.GetString("loggedInState");

    public IActionResult Index() => View();

    [HttpPost]
    public IActionResult Index(AdminLogin adminLogin)
    {
        var login = adminLogin.Username == "admin" && adminLogin.Password == "admin";

        if (!login)
        {
            ModelState.AddModelError("Login Failed", "Login failed, please try again.");
            return View(adminLogin);
        }

        // Login admin.
        HttpContext.Session.SetString("loggedInState", "loggedIn");
        return RedirectToAction("Index", "Home");
    }

    public IActionResult Logout()
    {
        // Logout customer.
        HttpContext.Session.Clear();

        return RedirectToAction("Index", "Home");
    }

}
