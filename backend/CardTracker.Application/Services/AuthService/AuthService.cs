using MediatR;
using System.Net;
using System.Threading.Tasks;
using CardTracker.Domain.Responses;
using CardTracker.Domain.Abstractions;
using CardTracker.Domain.Requests.Auth;
using CardTracker.Domain.Responses.Auth;
using CardTracker.Infrastructure.Abstractions.Identity;
using CardTracker.Application.Queries.UserQueries.GetUserByEmail;

namespace CardTracker.Application.Services.AuthService;

public class AuthService(IMediator mediator, IPasswordHasher passwordHasher) : IAuthService
{
    private readonly IMediator _mediator = mediator;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    public async Task<IBaseResponse<AuthResponse>> LoginUserAsync(LoginRequest request)
    {
        var query = new GetUserByEmailQuery(request.Email);

        var result = await _mediator.Send(query);

        if (result.IsSuccess is false)
            return new BaseResponse<AuthResponse>().Error(result.ErrorMessage, HttpStatusCode.Unauthorized);

        var verifyPasswordHash = _passwordHasher.VerifyPasswordHash(request.Password, result.Data.PasswordHash, result.Data.PasswordSalt);

        if (!verifyPasswordHash)
            return new BaseResponse<AuthResponse>().Error("Invalid password!", HttpStatusCode.Unauthorized);

        return new BaseResponse<AuthResponse>();
    }
}