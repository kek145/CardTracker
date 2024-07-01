using CardTracker.Domain;
using CardTracker.Application;
using CardTracker.Infrastructure;
using CardTracker.Api.Extensions;
using CardTracker.Application.Common.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAuthenticationAndAuthorization(builder.Configuration);

builder.Services.AddDomain().AddApplication().AddInfrastructure(builder.Configuration);

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(nameof(JwtOptions)));

builder.Services.AddCors(options =>
{
    options.AddPolicy("Client", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Client");

app.UseAuthentication();
app.UseAuthorization();

app.AddGlobalErrorHandling();

app.ApplyMigrations();

app.MapControllers();

app.Run();