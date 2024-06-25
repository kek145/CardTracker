using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using CardTracker.Application.Services.AuthService;
using CardTracker.Application.Services.TokenService;
using CardTracker.Application.Services.RegistrationService;

namespace CardTracker.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        serviceCollection.AddValidatorsFromAssembly(assembly);

        serviceCollection.AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(assembly));

        serviceCollection.AddTransient<IAuthService, AuthService>();
        serviceCollection.AddTransient<ITokenService, TokenService>();
        serviceCollection.AddTransient<IRegistrationService, RegistrationService>();
        
        return serviceCollection;
    }
}