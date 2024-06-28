using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CardTracker.Application.Services.TokenService;
using Microsoft.AspNetCore.Http;

namespace CardTracker.Api.Controllers;

[ApiController]
[Route("api")]
public class RefreshTokenController(ITokenService tokenService) : ControllerBase
{
    private readonly ITokenService _tokenService = tokenService;

    [HttpPost]
    [Route("revoke")]
    public async Task<IActionResult> RevokeRefreshToken()
    {
        return Ok();
    }
    
    [HttpPost]
    [Route("refresh-token")]
    public async Task<IActionResult> RefreshToken()
    {
        var refreshToken = HttpContext.Request.Cookies["X-Refresh-Token"];

        if (refreshToken is null)
            return NotFound(new { error = "Refresh token not found!" });
        
        var response = await _tokenService.ValidationRefreshTokenAsync(refreshToken);

        if (!response.IsSuccess)
            return Unauthorized(new { error = response.ErrorMessage });

        if (response.ErrorMessage == "Refresh token has been revoked!")
            return Forbid();
        
        HttpContext.Response.Cookies.Append("X-Refresh-Token", response.Data.RefreshToken, new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(30)
        });
        
        HttpContext.Response.Cookies.Append("X-Access-Token", response.Data.AccessToken, new CookieOptions
        {
            HttpOnly = true,
            SameSite = SameSiteMode.Strict,
            Expires = DateTimeOffset.UtcNow.AddDays(30)
        });
        
        return Ok(new
        {
            access_token = response.Data.AccessToken,
            refresh_token = response.Data.RefreshToken
        });
    }
}