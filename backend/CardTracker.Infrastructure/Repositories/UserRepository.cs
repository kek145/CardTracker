using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CardTracker.Domain.DTOs;
using CardTracker.Domain.Models;
using Microsoft.EntityFrameworkCore;
using CardTracker.Infrastructure.DataStore;
using CardTracker.Domain.Abstractions.Repositories;

namespace CardTracker.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<int> AddUserAsync(User user, CancellationToken ct)
    {
        var newUser = await _context.Users.AddAsync(user, ct);
        await _context.SaveChangesAsync(ct);

        return newUser.Entity.Id;
    }

    public async Task<User?> GetUserByIdAsync(int userId, CancellationToken ct)
    {
        var result = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == userId, ct);

        return result;
    }

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken ct)
    {
        var result = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email, ct);
        
        return result;
    }

    public async Task<int> UpdateVerificationStatusAsync(int userId, CancellationToken ct)
    {
        return await _context.Users
            .Where(x => x.Id == userId)
            .ExecuteUpdateAsync(x => 
                x.SetProperty(e => e.IsEmailConfirmed, true)
                    .SetProperty(v => v.VerifiedAt, DateTime.UtcNow)
                    .SetProperty(t => t.VerificationToken, (string?)null)
                    .SetProperty(e => e.VerificationTokenExpiry, (DateTime?)null), ct);
    }

    public async Task<int> UpdateForgotPasswordTokenAsync(string email, CancellationToken ct)
    {
        return await _context.Users
            .Where(u => u.Email == email)
            .ExecuteUpdateAsync(x => 
                    x.SetProperty(r => r.ResetToken, Guid.NewGuid().ToString())
                        .SetProperty(e => e.ResetTokenExpiry, DateTime.UtcNow.AddHours(1)), 
                ct);
    }

    public async Task<UserResetTokenDto?> GetUserResetTokenAsync(string token, CancellationToken ct = default)
    {
        var result = await _context.Users
            .Where(x => x.ResetToken == token)
            .Select(x => new UserResetTokenDto(
                x.Id,
                x.ResetToken,
                x.ResetTokenExpiry))
            .FirstOrDefaultAsync(ct);

        return result;
    }

    public async Task<UserVerificationDto?> GetVerificationTokenAsync(string token, CancellationToken ct)
    {
        var result = await _context.Users
            .AsNoTracking()
            .Where(t => t.VerificationToken == token)
            .Select(x => new UserVerificationDto(
                x.Id,
                x.IsEmailConfirmed, 
                x.VerificationToken, 
                x.VerifiedAt, 
                x.VerificationTokenExpiry))
            .FirstOrDefaultAsync(ct);

        return result;
    }

    public async Task<int> UpdatePasswordAsync(int userId, byte[] passwordHash, byte[] passwordSalt, CancellationToken ct = default)
    {
        return await _context.Users
            .Where(x => x.Id == userId)
            .ExecuteUpdateAsync(x => 
                x.SetProperty(h => h.PasswordHash, passwordHash)
                    .SetProperty(s => s.PasswordSalt, passwordSalt)
                    .SetProperty(t => t.ResetToken, (string?)null)
                    .SetProperty(e => e.ResetTokenExpiry, (DateTime?)null), ct);
    }
}