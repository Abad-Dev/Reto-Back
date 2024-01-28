using back.Models;
using back.Services;
using Microsoft.AspNetCore.Mvc;

namespace back.Controllers;

[ApiController]
[Route("/[controller]")]
public class OrderController: ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    [Route("/createdb")]
    public ActionResult Testdb()
    {
        _orderService.CreateDb();
        return Ok();
    }

    [HttpGet]
    [Route("/[controller]/{id}")]
    public ActionResult GetOne(string id)
    {
        return Ok(_orderService.GetOrderById(id));
    }

    [HttpGet]
    public ActionResult Get()
    {
        return Ok(_orderService.GetAll());
    }

    [HttpPost]
    [Route("OrderEmpty")]
    public ActionResult CreateEmpty()
    {
        return Ok(_orderService.CreateEmptyOrder());
    }

    [HttpPost]
    public ActionResult CreateOrder([FromBody] Order order)
    {
        return Ok(_orderService.CreateOrder(order));
    }

    [HttpDelete]
    [Route("/[controller]/{id}")]
    public ActionResult DeleteOrder(string id)
    {
        if (_orderService.DeleteOrder(id))
        {
            return Ok();
        } else {
            return BadRequest();
        }

    }

    [HttpPut]
    [Route("/add-detail")]
    public IActionResult AddProduct(string orderId, string productId, int qty)
    {
        if (_orderService.AddDetail(orderId, productId, qty))
        {
            return Ok();
        } else {
            return BadRequest();
        }
    }
}