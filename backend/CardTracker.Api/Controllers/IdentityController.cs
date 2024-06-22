using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardTracker.Api.Controllers;

[ApiController]
[Route("api/identity")]
public class IdentityController : ControllerBase
{
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login()
    {
        return Ok();
    }

    [HttpPost]
    [Route("registration")]
    public async Task<IActionResult> Registration()
    {
        return Ok();
    }

    [HttpGet]
    [Authorize]
    [Route("id")]
    public IActionResult GetUserId()
    {
        return Ok(new { userId = 1 });
    }
}