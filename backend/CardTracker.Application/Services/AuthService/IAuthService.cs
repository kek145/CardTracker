using System.Threading.Tasks;
using CardTracker.Domain.Abstractions;
using CardTracker.Domain.Requests.Auth;
using CardTracker.Domain.Responses.Auth;
using CardTracker.Domain.Responses.Common;

namespace CardTracker.Application.Services.AuthService;

public interface IAuthService
{
    Task<BaseResponse<AuthResponse>> LoginUserAsync(LoginRequest request);
}