using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using CardTracker.Domain.Requests.Account;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CardTracker.Application.Services.AccountService;
using Microsoft.AspNetCore.Http.HttpResults;

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
            return BadRequest(new { error = response.Message });
        
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
        var userId = Convert.ToInt32(HttpContext.User.FindFirst("UserId")?.Value);
        
        var response = await _accountService.ResetPasswordAsync(userId, request);

        return response.StatusCode switch
        {
            HttpStatusCode.BadRequest => BadRequest(new { error = response.Message }),
            HttpStatusCode.NotFound => NotFound(new { eror = response.Message }),
            _ => Ok(new { message = "Success" })
        };
    }
}