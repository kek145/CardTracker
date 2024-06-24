using System;
using System.Net;
using System.Threading.Tasks;
using CardTracker.Application.Services.RegistrationService;
using CardTracker.Domain.Requests.Registration;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardTracker.Api.Controllers;

[ApiController]
[Route("api/identity")]
public class IdentityController(IRegistrationService registrationService) : ControllerBase
{
    private readonly IRegistrationService _registrationService = registrationService;
    
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login()
    {
        return Ok();
    }

    [HttpPost]
    [Route("registration")]
    public async Task<IActionResult> Registration([FromBody] RegistrationRequest request)
    {
        var response = await _registrationService.RegistrationUserAsync(request);

        if (response.StatusCode == HttpStatusCode.BadRequest)
            return BadRequest(response.Message);
        
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