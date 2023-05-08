using Microsoft.AspNetCore.Mvc;
using AdminWebsite.Models;
using AdminWebsite.Models.DataManager;

namespace AdminWebsite.Controllers;

// See here for more information:
// https://docs.microsoft.com/en-au/aspnet/core/web-api/?view=aspnetcore-7.0

[ApiController]
[Route("api/[controller]")]
public class LoginController : ControllerBase
{
    private readonly LoginManager _repo;

    public LoginController(LoginManager repo)
    {
        _repo = repo;
    }

    // GET: api/login
    [HttpGet]
    public IEnumerable<Login> Get()
    {
        return _repo.GetAll();
    }

    // GET api/login/1
    [HttpGet("{id}")]
    public Login Get(string id)
    {
        return _repo.Get(id);
    }

    // POST api/login
    [HttpPost]
    public void Post([FromBody] Login login)
    {
        _repo.Add(login);
    }

    // PUT api/login
    [HttpPut]
    public void Put([FromBody] Login login)
    {
        _repo.Update(login.LoginID, login);
    }

    // DELETE api/login/1
    [HttpDelete("{id}")]
    public string Delete(string id)
    {
        return _repo.Delete(id);
    }
}
