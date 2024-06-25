using System;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CardTracker.Api.Extensions;

public static class TokenValidationParametersExtension
{
    public static TokenValidationParameters AddTokenParameters(IConfiguration configuration)
    {
        var key = configuration.GetSection("JWTConfiguration:SecretKey").Value!;
        var issuer = configuration.GetSection("JWTConfiguration:Issuer").Value!; 
        var audience = configuration.GetSection("JWTConfiguration:Audience").Value!;
        
        return new TokenValidationParameters
        {
            RequireAudience = true,
            RequireExpirationTime = true,
            ValidateAudience = true,
            ValidAudience = audience,
            ValidateIssuer = true,
            ValidIssuer = issuer,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
        };
    }
}