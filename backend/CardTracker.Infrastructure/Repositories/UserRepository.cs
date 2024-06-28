using System.Threading;
using System.Threading.Tasks;
using CardTracker.Domain.Models;
using Microsoft.EntityFrameworkCore;
using CardTracker.Infrastructure.DataStore;
using CardTracker.Infrastructure.Abstractions.Repositories;

namespace CardTracker.Infrastructure.Repositories;

public class UserRepository(ApplicationDbContext context) : IUserRepository
{
    private readonly ApplicationDbContext _context = context;

    public async Task<int> AddUserAsync(User user, CancellationToken cancellationToken)
    {
        var newUser = await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return newUser.Entity.Id;
    }

    public async Task<User?> GetUserByIdAsync(int userId, CancellationToken cancellationToken = default)
    {
        var result = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

        return result;
    }

    public async Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var result = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
        
        return result;
    }
}