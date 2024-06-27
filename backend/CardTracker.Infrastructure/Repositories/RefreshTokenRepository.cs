using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CardTracker.Domain.Models;
using CardTracker.Infrastructure.Abstractions.Repositories;
using CardTracker.Infrastructure.DataStore;
using Microsoft.EntityFrameworkCore;

namespace CardTracker.Infrastructure.Repositories;

public class RefreshTokenRepository(ApplicationDbContext context) : IRefreshTokenRepository
{
    private readonly ApplicationDbContext _context = context;
    
    public async Task<int> DeleteRefreshTokenAsync(int tokenId, CancellationToken cancellationToken = default)
    {
        return await _context.RefreshTokens
            .Where(x => x.Id == tokenId)
            .ExecuteDeleteAsync(cancellationToken);
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