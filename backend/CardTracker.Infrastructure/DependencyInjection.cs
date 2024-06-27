using CardTracker.Infrastructure.Abstractions.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using CardTracker.Infrastructure.DataStore;
using CardTracker.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using CardTracker.Infrastructure.Abstractions.Repositories;
using CardTracker.Infrastructure.Hasher;

namespace CardTracker.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection serviceCollection, IConfiguration configuration)
    {
        serviceCollection.AddScoped<IUserRepository, UserRepository>();
        serviceCollection.AddScoped<IPasswordHasher, PasswordHasher>();
        serviceCollection.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();
        
        serviceCollection.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
                .UseSnakeCaseNamingConvention();
        });
        
        return serviceCollection;
    }
}