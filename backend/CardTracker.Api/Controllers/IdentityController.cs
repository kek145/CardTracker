using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CardTracker.Domain.Requests.Auth;
using CardTracker.Domain.Responses.Auth;
using Microsoft.AspNetCore.Authorization;
using CardTracker.Domain.Requests.Registration;
using CardTracker.Application.Services.AuthService;
using CardTracker.Application.Services.RegistrationService;
using CardTracker.Application.Services.TokenService;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace CardTracker.Api.Controllers;

[ApiController]
[Route("api/identity")]
public class IdentityController(IAuthService authService, ITokenService tokenService, IRegistrationService registrationService) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IRegistrationService _registrationService = registrationService;
    
    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        var response = await _authService.LoginUserAsync(request);

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            return Unauthorized(new { message = response.Message });
        
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

    [HttpPost]
    [Route("registration")]
    public async Task<ActionResult<int>> Registration([FromBody] RegistrationRequest request)
    {
        var response = await _registrationService.RegistrationUserAsync(request);

        if (response.StatusCode == HttpStatusCode.BadRequest)
            return BadRequest(new { Error = response.Message });
        
        return Ok(new { UserId = response.Data });
    }

    [HttpPost]
    [Route("logout")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> Logout()
    {
        var refreshToken = HttpContext.Request.Cookies["X-Refresh-Token"];

        if (refreshToken is null)
            return NotFound(new { error = "Refresh token not found!" });
        
        var response = await _tokenService.RevokeTokenAsync(refreshToken);

        if (!response.IsSuccess)
            return Unauthorized(new { error = response.ErrorMessage });
        
        Response.Cookies.Delete("X-Refresh-Token");
        Response.Cookies.Delete("X-Access-Token");

        return Ok(new { message = "Logout successful" });
    }

    [HttpGet]
    [Route("id")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public ActionResult<int> GetById()
    {
        return Ok(new { id = 1 });
    }
}