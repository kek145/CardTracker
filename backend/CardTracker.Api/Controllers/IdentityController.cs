using System;
using System.Net;
using System.Threading.Tasks;
using CardTracker.Application.Services.AuthService;
using CardTracker.Application.Services.RegistrationService;
using CardTracker.Domain.Requests.Auth;
using CardTracker.Domain.Requests.Registration;
using CardTracker.Domain.Responses.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardTracker.Api.Controllers;

[ApiController]
[Route("api/identity")]
public class IdentityController(IAuthService authService, IRegistrationService registrationService) : ControllerBase
{
    private readonly IAuthService _authService = authService;
    private readonly IRegistrationService _registrationService = registrationService;
    
    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
    {
        var response = await _authService.LoginUserAsync(request);
        return Ok(response);
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

    [HttpGet]
    [Authorize]
    [Route("id")]
    public IActionResult GetUserId()
    {
        return Ok(new { userId = 1 });
    }
}