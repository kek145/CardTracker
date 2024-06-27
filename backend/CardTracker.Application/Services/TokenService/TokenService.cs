using System;
using MediatR;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using CardTracker.Domain.Responses.Auth;
using CardTracker.Application.Common.Models;
using CardTracker.Application.Common.Options;
using CardTracker.Application.Commands
    .RefreshTokenCommands.CreateRefreshToken;

namespace CardTracker.Application.Services.TokenService;

public class TokenService(IMediator mediator, IOptions<JwtOptions> options) : ITokenService
{
    private readonly IMediator _mediator = mediator;
    private readonly JwtOptions _options = options.Value;

    public async Task<bool> SaveTokenAsync(int userId, string refreshToken)
    {
        var command = new CreateRefreshTokenCommand(userId, refreshToken);

        Console.WriteLine(userId);
        
        var result = await _mediator.Send(command);

        return result.IsSuccess;
    }

    public AuthResponse GenerateTokens(UserPayload payload)
    {
        var accessToken = GenerateAccessToken(payload);
        var refreshToken = GenerateRefreshToken(155);

        return new AuthResponse
        {
            AccessToken = accessToken,
            RefreshToken = refreshToken
        };
    }

    private string GenerateAccessToken(UserPayload payload)
    {
        var jwtTokenHandler = new JwtSecurityTokenHandler();

        var secretKey = Encoding.UTF8.GetBytes(_options.SecretKey);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim("UserId", payload.UserId.ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, payload.FullName),
                new Claim(JwtRegisteredClaimNames.Email, payload.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToUniversalTime().ToString(CultureInfo.CurrentCulture))
            }),
            Expires = DateTime.UtcNow.AddHours(_options.ExpiresHours),
            Issuer = _options.Issuer,
            Audience = _options.Audience,
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256)
        };

        var token = jwtTokenHandler.CreateToken(tokenDescriptor);

        return jwtTokenHandler.WriteToken(token);
    }

    private static string GenerateRefreshToken(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, length).Select(s => s[random.Next(s.Length)]).ToArray());
    }
}