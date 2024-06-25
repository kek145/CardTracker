using System.Threading;
using System.Threading.Tasks;
using CardTracker.Domain.Models;

namespace CardTracker.Infrastructure.Abstractions.Repositories;

public interface IUserRepository
{
    Task<int> AddUserAsync(User user, CancellationToken cancellationToken = default);
    Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
}