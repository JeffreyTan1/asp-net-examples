using System.Text;
using Microsoft.AspNetCore.Mvc;
using MvcStore.Models;
using Newtonsoft.Json;

namespace MvcStore.Controllers;

public class OrdersController : Controller
{
    private readonly IHttpClientFactory _clientFactory;
    private HttpClient Client => _clientFactory.CreateClient();

    public OrdersController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

    // GET: Orders/Index
    public async Task<IActionResult> Index()
    {
        var response = await Client.GetAsync("api/orders");

        if(!response.IsSuccessStatusCode)
            throw new Exception();

        // Storing the response details received from web api.
        var result = await response.Content.ReadAsStringAsync();

        // Deserializing the response received from web api and storing into a list.
        var orders = JsonConvert.DeserializeObject<List<OrderDTO>>(result);

        return View(orders);
    }
    
}
