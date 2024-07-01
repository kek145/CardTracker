using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CardTracker.Domain.Requests.Account;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CardTracker.Application.Services.AccountService;

namespace CardTracker.Api.Controllers;

[ApiController]
[Route("api/account")]
[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
public class AccountController(IAccountService accountService) : ControllerBase
{
    private readonly IAccountService _accountService = accountService;
    
    [HttpPost]
    [Route("verify")]
    public async Task<IActionResult> VerifyAccount([FromQuery] VerificationTokenRequest request)
    {
        var response = await _accountService.VerifyAccountAsync(request);

        if (response.StatusCode == HttpStatusCode.BadRequest)
            return BadRequest(new { error = response.Errors });
        
        return Ok(new { message = $"Welcome back!" });
    }

    [HttpPost]
    [Route("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordRequest request)
    {
        var response = await _accountService.ForgotPasswordAsync(request);

        if (response.StatusCode == HttpStatusCode.NotFound)
            return NotFound(new { error = response.Message });
        
        return Ok(new { message = response.Message });
    }

    [HttpPost]
    [Route("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var response = await _accountService.ResetPasswordAsync(request);
        
        return response.StatusCode is HttpStatusCode.BadRequest or HttpStatusCode.NotFound 
            ? StatusCode((int)response.StatusCode, new { error = response.Errors }) : Ok(new { message = "Success" });
    }
}