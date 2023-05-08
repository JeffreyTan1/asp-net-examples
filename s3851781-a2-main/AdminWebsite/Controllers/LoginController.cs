using System.Text;
using Microsoft.AspNetCore.Mvc;
using AdminWebsite.Models;
using Newtonsoft.Json;

namespace AdminWebsite.Controllers;

public class LoginController : Controller
{
    private readonly IHttpClientFactory _clientFactory;
    private HttpClient Client => _clientFactory.CreateClient();

    public LoginController(IHttpClientFactory clientFactory) => _clientFactory = clientFactory;

    public async Task<IActionResult> Index()
    {
        var response = await Client.GetAsync("http://localhost:5000/api/login");
        if(!response.IsSuccessStatusCode)
            throw new Exception();
        var result = await response.Content.ReadAsStringAsync();
        var logins = JsonConvert.DeserializeObject<List<LoginDTO>>(result);

        return View(logins);
    }

    [HttpPost]
    public async Task<IActionResult> ToggleBlockLogin(string id)
    {
        var loginRes = await Client.GetAsync("http://localhost:5000/api/login/" + id);
        
        if (!loginRes.IsSuccessStatusCode)
            throw new Exception();
        var result = await loginRes.Content.ReadAsStringAsync();
        var login = JsonConvert.DeserializeObject<LoginDTO>(result);

        if (login == null)
            return NotFound();

        login.Blocked = !login.Blocked;
        var content = new StringContent(JsonConvert.SerializeObject(login), Encoding.UTF8, "application/json");
        Console.WriteLine(content);


        var updateRes = await Client.PutAsync("http://localhost:5000/api/login/", content);
        if (updateRes.IsSuccessStatusCode)
            return RedirectToAction("Index");
        else
        {
            throw new Exception();
        }
        
    }

}
