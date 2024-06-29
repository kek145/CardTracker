using System.Threading;
using System.Threading.Tasks;
using CardTracker.Domain.Models;

namespace CardTracker.Domain.Abstractions.Repositories;

public interface ITokenRepository
{
    Task<int> DeleteRefreshTokenAsync(int tokenId, CancellationToken cancellationToken = default);
    Task<int> RevokeRefreshTokenAsync(int tokenId, CancellationToken cancellationToken = default);
    Task<int> AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default);
    Task<RefreshToken?> GetRefreshTokenByUserIdAsync(int userId, CancellationToken cancellationToken = default);
    Task<RefreshToken?> GetRefreshTokenByNameAsync(string refreshToken, CancellationToken cancellationToken = default);
    Task<int> UpdateRefreshTokenAsync(int tokenId, string refreshToken, CancellationToken cancellationToken = default);
}