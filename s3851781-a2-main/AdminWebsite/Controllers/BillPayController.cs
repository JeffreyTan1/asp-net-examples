using System.Text;
using Microsoft.AspNetCore.Mvc;
using AdminWebsite.Models;
using Newtonsoft.Json;

namespace AdminWebsite.Controllers;

public class BillPayController : Controller
{
    private readonly IHttpClientFactory _clientFactory;
    private HttpClient Client => _clientFactory.CreateClient();

    public BillPayController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

    public async Task<IActionResult> Index()
    {
        var response = await Client.GetAsync("http://localhost:5000/api/billPay");
        if(!response.IsSuccessStatusCode)
            throw new Exception();
        var result = await response.Content.ReadAsStringAsync();
        var billPays = JsonConvert.DeserializeObject<List<BillPayDTO>>(result);

        return View(billPays);
    }

    [HttpPost]
    public async Task<IActionResult> ToggleBillPayActive(int id)
    {
        var billpayRes = await Client.GetAsync("http://localhost:5000/api/billPay/" + id);
        if (!billpayRes.IsSuccessStatusCode)
            throw new Exception();
        var result = await billpayRes.Content.ReadAsStringAsync();
        var billPay = JsonConvert.DeserializeObject<BillPayDTO>(result);

        if (billPay == null)
            return NotFound();

        billPay.Active = !billPay.Active;
        var content = new StringContent(JsonConvert.SerializeObject(billPay), Encoding.UTF8, "application/json");
        var updateRes = await Client.PutAsync("http://localhost:5000/api/billPay/", content);
        if (updateRes.IsSuccessStatusCode)
                return RedirectToAction("Index");
        else
        {
            throw new Exception();
        }

    }
}
