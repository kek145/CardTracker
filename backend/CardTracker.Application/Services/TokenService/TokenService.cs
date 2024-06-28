using System;
using MediatR;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using CardTracker.Application.Common;
using System.IdentityModel.Tokens.Jwt;
using CardTracker.Domain.Responses.Auth;
using CardTracker.Application.Common.Models;
using CardTracker.Application.Common.Options;
using CardTracker.Application.Commands.TokenCommands.CreateToken;
using CardTracker.Application.Queries.TokenQueries.GetTokenByName;
using CardTracker.Application.Queries.UserQueries.GetUserById;

namespace CardTracker.Application.Services.TokenService;

public class TokenService(IMediator mediator, IOptions<JwtOptions> options) : ITokenService
{
    private readonly IMediator _mediator = mediator;
    private readonly JwtOptions _options = options.Value;

    public async Task<Result<AuthResponse>> ValidationRefreshTokenAsync(string refreshToken)
     {
        var query = new GetTokenByNameQuery(refreshToken);

        var token = await _mediator.Send(query);

        if (!token.IsSuccess)
            return Result<AuthResponse>.Failure($"{token.ErrorMessage}");

        if (token.Data.ExpiresAt < DateTime.UtcNow)
            return Result<AuthResponse>.Failure("Refresh token has expired!");

        if (token.Data.IsRevoked)
            return Result<AuthResponse>.Failure("Refresh token has been revoked!");

        var user = new GetUserByIdQuery(token.Data.UserId);

        var result = await _mediator.Send(user);
        
        if (!result.IsSuccess)
            return Result<AuthResponse>.Failure(result.ErrorMessage);

        var payload = new UserPayload
        {
            UserId = result.Data.Id,
            FullName = $"{result.Data.FirstName} {result.Data.LastName}",
            Email = result.Data.Email
        };
        
        var tokens = GenerateTokens(payload);

        await SaveTokenAsync(result.Data.Id, tokens.RefreshToken);
        
        return Result<AuthResponse>.Success(tokens);
    }

    public async Task<bool> SaveTokenAsync(int userId, string refreshToken)
    {
        var command = new CreateTokenCommand(userId, refreshToken);
        
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
            Expires = DateTime.UtcNow.AddMinutes(_options.ExpiresHours),
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