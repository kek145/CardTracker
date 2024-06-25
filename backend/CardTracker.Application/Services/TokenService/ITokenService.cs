using CardTracker.Application.Common.Models;
using CardTracker.Domain.Responses.Auth;

namespace CardTracker.Application.Services.TokenService;

public interface ITokenService
{
    AuthResponse GenerateTokens(UserPayload payload);
}