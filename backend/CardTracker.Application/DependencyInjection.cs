using Microsoft.Extensions.DependencyInjection;

namespace CardTracker.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
    {
        return serviceCollection;
    }
}