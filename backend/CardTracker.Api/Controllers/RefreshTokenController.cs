using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using CardTracker.Application.Services.TokenService;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CardTracker.Api.Controllers;

[ApiController]
[Route("api")]
public class RefreshTokenController(ITokenService tokenService) : ControllerBase
{
    private readonly ITokenService _tokenService = tokenService;

    [HttpPost]
    [Route("revoke")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> RevokeRefreshToken()
    {
        var refreshToken = HttpContext.Request.Cookies["X-Refresh-Token"];

        if (refreshToken is null)
            return NotFound(new { error = "Refresh token not found!" });
        
        var response = await _tokenService.RevokeTokenAsync(refreshToken);

        if (response is { IsSuccess: false, ErrorMessage: "refresh token has expired" })
            return Unauthorized(new { error = response.ErrorMessage });
        
        Response.Cookies.Delete("X-Refresh-Token");
        
        return Ok(new { message = "Token successfully revoked!" });
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