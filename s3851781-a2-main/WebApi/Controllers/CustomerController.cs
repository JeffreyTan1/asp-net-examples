using Microsoft.AspNetCore.Mvc;
using AdminWebsite.Models;
using AdminWebsite.Models.DataManager;

namespace AdminWebsite.Controllers;

// See here for more information:
// https://docs.microsoft.com/en-au/aspnet/core/web-api/?view=aspnetcore-7.0

[ApiController]
[Route("api/[controller]")]
public class CustomerController : ControllerBase
{
    private readonly CustomerManager _repo;

    public CustomerController(CustomerManager repo)
    {
        _repo = repo;
    }

    // GET: api/customer
    [HttpGet]
    public IEnumerable<Customer> Get()
    {
        return _repo.GetAll();
    }

    // GET api/customer/1
    [HttpGet("{id}")]
    public Customer Get(int id)
    {
        return _repo.Get(id);
    }

    // POST api/customer
    [HttpPost]
    public void Post([FromBody] Customer customer)
    {
        _repo.Add(customer);
    }

    // PUT api/customer
    [HttpPut]
    public void Put([FromBody] Customer customer)
    {
        _repo.Update(customer.CustomerID, customer);
    }

    // DELETE api/customer/1
    [HttpDelete("{id}")]
    public long Delete(int id)
    {
        return _repo.Delete(id);
    }
}
