using back.Models;
using back.Services;
using Microsoft.AspNetCore.Mvc;

namespace back.Controllers;

[ApiController]
[Route("/[controller]")]
public class DetailController: ControllerBase
{
    private readonly IDetailService _DetailService;

    public DetailController(IDetailService DetailService)
    {
        _DetailService = DetailService;
    }

    [HttpGet]
    public ActionResult Get()
    {
        return Ok(_DetailService.GetAll());
    }

    [HttpPost]
    public ActionResult Post([FromBody] Detail Detail)
    {
        _DetailService.Create(Detail);
        return Ok();
    }
}