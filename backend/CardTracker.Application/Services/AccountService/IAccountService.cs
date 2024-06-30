using System.Threading.Tasks;
using CardTracker.Domain.Requests.Account;
using CardTracker.Domain.Responses.Common;

namespace CardTracker.Application.Services.AccountService;

public interface IAccountService
{
    Task<BaseResponse> ForgotPasswordAsync(ForgotPasswordRequest request);
    Task<BaseResponse> VerifyAccountAsync(VerificationTokenRequest request);
    Task<BaseResponse> ResetPasswordAsync(ResetPasswordRequest request);
}
