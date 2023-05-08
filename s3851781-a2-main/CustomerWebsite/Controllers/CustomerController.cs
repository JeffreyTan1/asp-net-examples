using Microsoft.AspNetCore.Mvc;
using CustomerWebsite.Data;
using CustomerWebsite.Models;
using Microsoft.EntityFrameworkCore;
namespace CustomerWebsite.Controllers;

using ImageMagick;
using NuGet.ContentModel;
using System.Web;
using Newtonsoft.Json;

public class CustomerController : Controller
{
  private readonly McbaContext _context;

  private int CustomerID => HttpContext.Session.GetInt32(nameof(Customer.CustomerID)).Value;

  public CustomerController(McbaContext context) => _context = context;

  public async Task<IActionResult> Index() => View(await _context.Customer.Include(x => x.Accounts).ThenInclude(a => a.AccountTransactions).FirstOrDefaultAsync((x => x.CustomerID == CustomerID)));

  public async Task<IActionResult> CustomerInformation()
  {
    var customer = await _context.Customer.FirstOrDefaultAsync(x => x.CustomerID == CustomerID);
    if (customer == null) { return NotFound(); }
    return View(customer);
  }

    public async Task<IActionResult> UpdateCustomerInformation()
    {
        var customer = await _context.Customer.FirstOrDefaultAsync(x => x.CustomerID == CustomerID);
        if (customer == null) { return NotFound(); }
        var updateCustomer = new UpdateCustomerViewModel
        {
            Name = customer.Name,
            TFN = customer.TFN,
            Address = customer.Address,
            City = customer.City,
            State = customer.State,
            PostCode = customer.PostCode,
            Mobile = customer.Mobile,

        };
        return View(updateCustomer);
    }

    [HttpPost]
  public async Task<IActionResult> UpdateCustomerInformation(UpdateCustomerViewModel customer)
  {
 
        if (!ModelState.IsValid)
        {
            return View(customer);
        }
        
        var customerToUpdate = await _context.Customer.FirstOrDefaultAsync(x => x.CustomerID == CustomerID);
        if (customerToUpdate == null) { 
                return NotFound(); 
        }
        customerToUpdate.Name = customer.Name;
        customerToUpdate.TFN = customer.TFN;
        customerToUpdate.Address = customer.Address;
        customerToUpdate.City = customer.City;
        customerToUpdate.State = customer.State;
        customerToUpdate.PostCode = customer.PostCode;
        customerToUpdate.Mobile = customer.Mobile;
        

        _context.Update(customerToUpdate);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(CustomerInformation));
  }

    public IActionResult UpdateProfilePicture()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> DeleteProfilePicture()
    {
        var customer = await _context.Customer.FirstOrDefaultAsync(x => x.CustomerID == CustomerID);
        if (customer == null) { return NotFound(); }
        customer.ImageBase64 = null;
        HttpContext.Session.SetString("ImageBase64", string.Empty);
        _context.Update(customer);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(CustomerInformation));
    }
    
    [HttpPost]
    public async Task<IActionResult> UploadProfilePicture(ImagePayload payload)
    {
        var customerToUpdate = await _context.Customer.FirstOrDefaultAsync(x => x.CustomerID == CustomerID);
        if (customerToUpdate == null) { return NotFound(); }

        // Convert base64 string to byte[]
        // split the string into two parts, the first part is the data type, the second part is the base64 string
        var split = payload.Image.Split(',');
        var base64Data = split[1];

        var imageBytes = Convert.FromBase64String(base64Data);
        // read into a MagickImage
        using var magickImage = new MagickImage(imageBytes);
        // if image is square, resize to 400x400
        if (magickImage.Width == magickImage.Height)
        {
            magickImage.Resize(400, 400);
        }
        // if image is landscape make width 400 and height proportional so it is 1:1 aspect ratio
        else if (magickImage.Width > magickImage.Height)
        {
            magickImage.Resize(400, 0);
        }
        // if image is portrait make height 400 and width proportional so it is 1:1 aspect ratio
        else
        {
            magickImage.Resize(0, 400);
        }

        // convert magickImage to base64 string
        // base64 jpeg suffix
        var suffix = "data:image/jpeg;base64,";
        var newBase64String = suffix + magickImage.ToBase64();
        
        customerToUpdate.ImageBase64 = newBase64String;
        _context.Update(customerToUpdate);
        await _context.SaveChangesAsync();

        HttpContext.Session.SetString("ImageBase64", newBase64String ?? string.Empty);

        

        return RedirectToAction(nameof(CustomerInformation));
    }

}

