using Microsoft.AspNetCore.Mvc;
using MvcStore.Data;
using MvcStore.Models;


namespace MvcStore.Api.Controllers;

// See here for more information:
// https://docs.microsoft.com/en-au/aspnet/core/web-api/?view=aspnetcore-7.0

[ApiController]
[Route("api/orders")]
public class OrdersApiController : ControllerBase
{
    private readonly MvcStoreContext _context;

    public OrdersApiController(MvcStoreContext context)
    {
        _context = context;
    }
    
    // GET: api/orders
    [HttpGet]
    public IEnumerable<OrderDTO> Get()
    {
        var orders = _context.Order.Select(x => new OrderDTO
        {
            OrderID = x.OrderID,
            OrderDate = x.OrderDate,
            CustomerName = x.CustomerName,
            DeliveryAddress = x.DeliveryAddress,
            DeliveredDate = x.DeliveredDate,
            Quantity = x.OrderedProducts.Sum(y => y.Quantity)
        }).ToList();

        return orders;
    }
}
