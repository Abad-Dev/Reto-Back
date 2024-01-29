using back.Models;
using back.Services;
using Microsoft.AspNetCore.Mvc;

namespace back.Controllers;

[ApiController]
[Route("/[controller]")]
public class ProductController: ControllerBase
{
    private readonly IProductService _productService;

    public ProductController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet]
    public ActionResult Get()
    {
        return Ok(_productService.GetAll());
    }

    [HttpPost]
    public ActionResult Post([FromBody] Product product)
    {
        _productService.Create(product);
        return Ok(product);
    }

    [HttpDelete]
    [Route("{id}")]
    public ActionResult Delete(string id)
    {
        if (_productService.Delete(id))
        {
            return Ok();
        } else {
            return BadRequest();
        }
    }
}