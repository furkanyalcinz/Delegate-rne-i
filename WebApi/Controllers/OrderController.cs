using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrderController(OrderService orderService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetOrder()
    {
        await orderService.Process( new Order{Name = "Test"} );
        return Ok();
    }
}
