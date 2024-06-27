using System.Threading.Tasks;
using CardTracker.Domain.Responses.Auth;
using CardTracker.Application.Common.Models;

namespace CardTracker.Application.Services.TokenService;

public interface ITokenService
{
    Task<bool> SaveTokenAsync(int userId, string refreshToken);
    AuthResponse GenerateTokens(UserPayload payload);
}