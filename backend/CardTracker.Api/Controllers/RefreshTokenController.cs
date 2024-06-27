using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CardTracker.Application.Services.TokenService;

namespace CardTracker.Api.Controllers;

[ApiController]
[Route("api")]
public class RefreshTokenController(ITokenService tokenService) : ControllerBase
{
    private readonly ITokenService _tokenService = tokenService;

    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        return Ok();
    }

    [HttpPost]
    [Route("revoke")]
    public async Task<IActionResult> RevokeRefreshToken()
    {
        return Ok();
    }
}