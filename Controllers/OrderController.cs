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
    public ActionResult Get()
    {
        _orderService.CreateDb();
        return Ok();
    }

    [HttpPost]
    public ActionResult Post()
    {
        _orderService.CreateEmptyOrder();
        return Ok();
    }

    [HttpPut]
    [Route("/addproduct")]
    public IActionResult AddProduct(string orderId, string productId)
    {
        _orderService.AddProductToOrder(orderId, productId);

        return Ok();
    }
}