using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using CardTracker.Infrastructure.DataStore;
using Microsoft.Extensions.DependencyInjection;

namespace CardTracker.Api.Extensions;

public static class MigrationExtension
{
    public static void ApplyMigrations(this IApplicationBuilder applicationBuilder)
    {
        using IServiceScope scope = applicationBuilder.ApplicationServices.CreateScope();

        using ApplicationDbContext dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>()!;

        dbContext.Database.Migrate();
    }
}