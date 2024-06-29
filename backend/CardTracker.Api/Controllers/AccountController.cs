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
            return BadRequest(new { error = response.Message });
        
        return Ok(new { message = $"Welcome back!" });
    }
}