using System.Threading.Tasks;
using CardTracker.Domain.Abstractions;
using CardTracker.Domain.Requests.Account;

namespace CardTracker.Application.Services.AccountService;

public interface IAccountService
{
    Task<IBaseResponse<string>> ForgotPasswordAsync(ForgotPasswordRequest request);
    Task<IBaseResponse<string>> VerifyAccountAsync(VerificationTokenRequest request);
    Task<IBaseResponse<string>> ResetPasswordAsync(int userId, ResetPasswordRequest request);
}
