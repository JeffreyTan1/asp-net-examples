using Microsoft.AspNetCore.Mvc;
using CustomerWebsite.Data;
using CustomerWebsite.Models;
using SimpleHashing.Net;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace CustomerWebsite.Controllers;

[AllowAnonymous]
public class LoginController : Controller
{
    private static readonly ISimpleHash s_simpleHash = new SimpleHash();
    private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;
    
    private readonly McbaContext _context;

    public LoginController(McbaContext context) => _context = context;

    public IActionResult Login() => View();

    [HttpPost]
    public async Task<IActionResult> Login(string loginID, string password)
    {
        var login = await _context.Login.Include(x => x.Customer).FirstOrDefaultAsync(x => x.LoginID == loginID);

        if (login == null || string.IsNullOrEmpty(password) || !s_simpleHash.Verify(password, login.PasswordHash))
        {
            ModelState.AddModelError("Login Failed", "Login failed, please try again.");
            return View(new Login { LoginID = loginID });
        }

        if (login.Blocked)
        {
            ModelState.AddModelError("Login Failed", "Login blocked, please contact your bank.");
            return View(new Login { LoginID = loginID });
        }

        // Login customer.
        HttpContext.Session.SetInt32(nameof(Customer.CustomerID), login.CustomerID);
        HttpContext.Session.SetString(nameof(Customer.Name), login.Customer.Name);
        HttpContext.Session.SetString("ImageBase64", login.Customer.ImageBase64 ?? string.Empty);

        return RedirectToAction("Index", "Customer");
    }

    public IActionResult Logout()
    {
        // Logout customer.
        HttpContext.Session.Clear();

        return RedirectToAction("Index", "Home");
    }

    [Authorize]
    public IActionResult UpdatePassword()
    {
        var updatePasswordViewModel = new UpdatePasswordViewModel { };
        return View(updatePasswordViewModel);
    }

    [Authorize]
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> UpdatePassword(UpdatePasswordViewModel payload)
    {
        if (!ModelState.IsValid)
        {
            return View(payload);
        }

        var login = _context.Login.FirstOrDefault(x => x.CustomerID == CustomerID);
        var newPassword = payload.Password;
        var newPasswordHash = s_simpleHash.Compute(newPassword);
        login.PasswordHash = newPasswordHash;
        _context.Login.Update(login);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Customer");
    }
}
