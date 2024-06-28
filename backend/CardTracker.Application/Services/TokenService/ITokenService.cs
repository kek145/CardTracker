using System.Threading.Tasks;
using CardTracker.Application.Common;
using CardTracker.Domain.Responses.Auth;
using CardTracker.Application.Common.Models;
using CardTracker.Domain.Abstractions;

namespace CardTracker.Application.Services.TokenService;

public interface ITokenService
{
    AuthResponse GenerateTokens(UserPayload payload);
    Task<bool> SaveTokenAsync(int userId, string refreshToken);
    Task<Result<AuthResponse>> ValidationRefreshTokenAsync(string refreshToken);
}