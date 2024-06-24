using CardTracker.Api.Middlewares;
using Microsoft.AspNetCore.Builder;

namespace CardTracker.Api.Extensions;

public static class ApplicationBuilderExtension
{
    public static IApplicationBuilder AddGlobalErrorHandling(this IApplicationBuilder applicationBuilder)
        => applicationBuilder.UseMiddleware<ErrorHandlingMiddleware>();
}