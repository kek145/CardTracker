using Microsoft.AspNetCore.Mvc;

namespace CardTracker.Api.Controllers;

[ApiController]
[Route("api/[controller]/[action]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult GetUserName()
    {
        return Ok(new { data = "kek145" });
    }
}