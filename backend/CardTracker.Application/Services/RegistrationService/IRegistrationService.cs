using System.Threading.Tasks;
using CardTracker.Domain.Abstractions;
using CardTracker.Domain.Requests.Registration;
using CardTracker.Domain.Responses.Common;

namespace CardTracker.Application.Services.RegistrationService;

public interface IRegistrationService
{
    Task<BaseResponse<int>> RegistrationUserAsync(RegistrationRequest request);
}