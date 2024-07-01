using System.Threading.Tasks;
using CardTracker.Domain.Common;
using CardTracker.Domain.Responses.Auth;
using CardTracker.Application.Common.Models;

namespace CardTracker.Application.Services.TokenService;

public interface ITokenService
{
    Task<Result> RevokeTokenAsync(string refreshToken);
    AuthResponse GenerateTokens(UserPayload payload);
    Task<bool> SaveTokenAsync(int userId, string refreshToken);
    Task<Result<AuthResponse>> ValidationRefreshTokenAsync(string refreshToken);
}