using System.Threading;
using System.Threading.Tasks;
using CardTracker.Domain.Models;

namespace CardTracker.Infrastructure.Abstractions;

public interface IUserRepository
{
    Task<int> AddUserAsync(User user, CancellationToken cancellationToken = default);
    Task<bool> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
}