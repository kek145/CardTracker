using MediatR;
using System.Net;
using System.Threading.Tasks;
using CardTracker.Application.Common.Models;
using CardTracker.Domain.Responses;
using CardTracker.Domain.Abstractions;
using CardTracker.Domain.Requests.Auth;
using CardTracker.Domain.Responses.Auth;
using CardTracker.Infrastructure.Abstractions.Identity;
using CardTracker.Application.Queries.UserQueries.GetUserByEmail;
using CardTracker.Application.Services.TokenService;

namespace CardTracker.Application.Services.AuthService;

public class AuthService(IMediator mediator, ITokenService tokenService, IPasswordHasher passwordHasher) : IAuthService
{
    private readonly IMediator _mediator = mediator;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    public async Task<IBaseResponse<AuthResponse>> LoginUserAsync(LoginRequest request)
    {
        var query = new GetUserByEmailQuery(request.Email);

        var user = await _mediator.Send(query);

        if (user.IsSuccess is false)
            return new BaseResponse<AuthResponse>().Error(user.ErrorMessage, HttpStatusCode.Unauthorized);

        var verifyPasswordHash = _passwordHasher.VerifyPasswordHash(request.Password, user.Data.PasswordHash, user.Data.PasswordSalt);

        if (!verifyPasswordHash)
            return new BaseResponse<AuthResponse>().Error("Invalid password!", HttpStatusCode.Unauthorized);

        var payload = new UserPayload
        {
            UserId = user.Data.Id,
            FullName = $"{user.Data.FirstName} {user.Data.LastName}",
            Email = user.Data.Email
        };

        var tokens = _tokenService.GenerateTokens(payload);

        var result = await _tokenService.SaveTokenAsync(user.Data.Id, tokens.RefreshToken);

        if (!result)
            return new BaseResponse<AuthResponse>().Error("An error occurred while generating the refresh token",
                HttpStatusCode.Unauthorized);
        
        return new BaseResponse<AuthResponse>().Success("Authentication success", HttpStatusCode.OK, tokens);
    }
}