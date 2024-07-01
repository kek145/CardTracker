using MediatR;
using System.Net;
using System.Threading.Tasks;
using CardTracker.Domain.Requests.Auth;
using CardTracker.Domain.Responses.Auth;
using CardTracker.Domain.Responses.Common;
using CardTracker.Application.Common.Models;
using CardTracker.Application.Services.TokenService;
using CardTracker.Infrastructure.Abstractions.Identity;
using CardTracker.Application.Queries.UserQueries.GetUserByEmail;

namespace CardTracker.Application.Services.AuthService;

public class AuthService(IMediator mediator, ITokenService tokenService, IPasswordHasher passwordHasher) : IAuthService
{
    private readonly IMediator _mediator = mediator;
    private readonly ITokenService _tokenService = tokenService;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    public async Task<BaseResponse<AuthResponse>> LoginUserAsync(LoginRequest request)
    {
        var query = new GetUserByEmailQuery(request.Email);
        var user = await _mediator.Send(query);

        if (user.IsSuccess is false || user.Data is null)
            return BaseResponse<AuthResponse>.Failure("Error", HttpStatusCode.Unauthorized, [user.ErrorMessage]);

        var verifyPasswordHash = _passwordHasher.VerifyPasswordHash(request.Password, user.Data.PasswordHash, user.Data.PasswordSalt);

        if (!verifyPasswordHash)
            return BaseResponse<AuthResponse>.Failure("Error", HttpStatusCode.Unauthorized, ["Invalid password!"]);

        var payload = new UserPayload
        {
            UserId = user.Data.Id,
            FullName = $"{user.Data.FirstName} {user.Data.LastName}",
            Email = user.Data.Email
        };

        var tokens = _tokenService.GenerateTokens(payload);

        var result = await _tokenService.SaveTokenAsync(user.Data.Id, tokens.RefreshToken);

        if (!result)
            return BaseResponse<AuthResponse>.Failure("Error",
                HttpStatusCode.Unauthorized, ["An error occurred while generating the refresh token"]);
        
        return BaseResponse<AuthResponse>.Success("Success", HttpStatusCode.OK, tokens);
    }
}