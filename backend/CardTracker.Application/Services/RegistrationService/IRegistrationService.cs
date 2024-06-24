using System.Threading.Tasks;
using CardTracker.Domain.Abstractions;
using CardTracker.Domain.Requests.Registration;

namespace CardTracker.Application.Services.RegistrationService;

public interface IRegistrationService
{
    Task<IBaseResponse<int>> RegistrationUserAsync(RegistrationRequest request);
}