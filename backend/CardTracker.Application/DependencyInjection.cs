﻿using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CardTracker.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
    {
        var assembly = typeof(DependencyInjection).Assembly;

        serviceCollection.AddValidatorsFromAssembly(assembly);

        serviceCollection.AddMediatR(configuration => configuration.RegisterServicesFromAssemblies(assembly));
        
        return serviceCollection;
    }
}