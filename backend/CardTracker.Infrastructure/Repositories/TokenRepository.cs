using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CardTracker.Domain.Models;
using Microsoft.EntityFrameworkCore;
using CardTracker.Infrastructure.DataStore;
using CardTracker.Domain.Abstractions.Repositories;

namespace CardTracker.Infrastructure.Repositories;

public class TokenRepository(ApplicationDbContext context) : ITokenRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<RefreshToken?> GetRefreshTokenByUserIdAsync(int userId, CancellationToken cancellationToken)
    {
        var token = await _context.RefreshTokens.AsNoTracking().FirstOrDefaultAsync(x => x.UserId == userId, cancellationToken);
        return token;
    }

    public async Task<RefreshToken?> GetRefreshTokenByNameAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var token = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken, cancellationToken);
        return token;
    }

    public async Task<int> DeleteRefreshTokenAsync(int tokenId, CancellationToken cancellationToken = default)
    {
        return await _context.RefreshTokens
            .Where(x => x.Id == tokenId)
            .ExecuteDeleteAsync(cancellationToken);
    }

    public async Task<int> RevokeRefreshTokenAsync(int tokenId, CancellationToken cancellationToken = default)
    {
        return await _context.RefreshTokens
            .Where(x => x.Id == tokenId)
            .ExecuteUpdateAsync(x 
                => x.SetProperty(r => r.IsRevoked, true)
                    .SetProperty(t => t.RevokedAt, DateTime.UtcNow), cancellationToken);
    }

    public async Task<int> UpdateRefreshTokenAsync(int tokenId, string refreshToken, CancellationToken cancellationToken = default)
    {
        return await _context.RefreshTokens
            .Where(x => x.Id == tokenId)
            .ExecuteUpdateAsync(x => 
                    x.SetProperty(t => t.Token, refreshToken), cancellationToken);
    }

    public async Task<int> AddRefreshTokenAsync(RefreshToken refreshToken, CancellationToken cancellationToken = default)
    {
        var result = await _context.RefreshTokens.AddAsync(refreshToken, cancellationToken);
        
        await _context.SaveChangesAsync(cancellationToken);
        
        return result.Entity.Id;
    }
}