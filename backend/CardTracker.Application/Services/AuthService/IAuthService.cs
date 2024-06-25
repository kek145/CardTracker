using System.Threading.Tasks;
using CardTracker.Domain.Abstractions;
using CardTracker.Domain.Requests.Auth;
using CardTracker.Domain.Responses.Auth;

namespace CardTracker.Application.Services.AuthService;

public interface IAuthService
{
    Task<IBaseResponse<AuthResponse>> LoginUserAsync(LoginRequest request);
}