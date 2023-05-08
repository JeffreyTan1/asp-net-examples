using System.Text;
using Microsoft.AspNetCore.Mvc;
using AdminWebsite.Models;
using AdminWebsite.ViewModels;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace AdminWebsite.Controllers;

public class CustomerController : Controller
{
    private readonly IHttpClientFactory _clientFactory;
    private HttpClient Client => _clientFactory.CreateClient();

    public CustomerController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

    public async Task<IActionResult> Index()
    {
        var response = await Client.GetAsync("http://localhost:5000/api/customer");
        if (!response.IsSuccessStatusCode)
            throw new Exception();
        var result = await response.Content.ReadAsStringAsync();
        var customers = JsonConvert.DeserializeObject<List<CustomerDTO>>(result);

        return View(customers);
    }

    public async Task<IActionResult> Edit(int id)
    {
        var response = await Client.GetAsync("http://localhost:5000/api/customer/" + id);
        if (!response.IsSuccessStatusCode)
            throw new Exception();
        var result = await response.Content.ReadAsStringAsync();
        var customer = JsonConvert.DeserializeObject<CustomerDTO>(result);
        if (customer == null)
            return NotFound();
        return View(customer);
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(CustomerDTO customer)
    {
        if (!ModelState.IsValid)
        {
            return View(customer);
        }
        var content = new StringContent(JsonConvert.SerializeObject(customer), Encoding.UTF8, "application/json");
        var response = await Client.PutAsync("http://localhost:5000/api/customer/", content);
        if (response.IsSuccessStatusCode)
            return RedirectToAction("Index");
        else
        {
            throw new Exception();
        }
    }

}
