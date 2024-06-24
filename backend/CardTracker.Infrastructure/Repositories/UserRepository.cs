using System.Threading;
using System.Threading.Tasks;
using CardTracker.Domain.Models;
using CardTracker.Infrastructure.Abstractions;
using CardTracker.Infrastructure.DataStore;
using Microsoft.EntityFrameworkCore;

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

    public async Task<bool> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        var result = await _context.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email, cancellationToken);

        return result is not null;
    }
}