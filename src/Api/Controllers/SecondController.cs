namespace Api.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("[controller]")]
public class SecondController : ControllerBase
{
    [HttpGet("Test")]
    public IActionResult Test()
    {
        return Ok();
    }
}