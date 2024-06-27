using System.Threading;
using System.Threading.Tasks;
using CardTracker.Domain.Models;

namespace CardTracker.Infrastructure.Abstractions.Repositories;

public interface IRefreshTokenRepository
{
    Task<RefreshToken?> GetRefreshTokenByUserIdAsync(int userId);
    Task<int> DeleteRefreshTokenAsync(int tokenId, CancellationToken cancellationToken = default);
    Task<int> UpdateRefreshTokenAsync(int tokenId, string refreshToken, CancellationToken cancellationToken = default);
    Task<int> AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
}