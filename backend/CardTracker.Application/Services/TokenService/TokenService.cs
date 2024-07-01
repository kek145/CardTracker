using System;
using MediatR;
using System.Text;
using System.Globalization;
using System.Threading.Tasks;
using System.Security.Claims;
using CardTracker.Domain.Common;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using CardTracker.Domain.Responses.Auth;
using CardTracker.Application.Common.Models;
using CardTracker.Application.Common.Options;
using CardTracker.Application.Queries.UserQueries.GetUserById;
using CardTracker.Application.Commands.TokenCommands.CreateToken;
using CardTracker.Application.Commands.TokenCommands.RevokeToken;
using CardTracker.Application.Queries.TokenQueries.GetTokenByName;

namespace CardTracker.Application.Services.TokenService;

public class TokenService(IMediator mediator, IOptions<JwtOptions> options) : ITokenService
{
    private readonly IMediator _mediator = mediator;
    private readonly JwtOptions _options = options.Value;

    public async Task<Result<AuthResponse>> ValidationRefreshTokenAsync(string refreshToken)
     {
        var query = new GetTokenByNameQuery(refreshToken);
        var token = await _mediator.Send(query);
        
        if (!token.IsSuccess || token.Data is null)
            return Result<AuthResponse>.Failure($"{token.ErrorMessage}");

        var user = new GetUserByIdQuery(token.Data.UserId);
        var result = await _mediator.Send(user);
        
        if (!result.IsSuccess)
            return Result<AuthResponse>.Failure(result.ErrorMessage);
        
        if(result.Data is null)
            return Result<AuthResponse>.Failure("User is null!");
        
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

    public async Task<Result> RevokeTokenAsync(string refreshToken)
    {
        var tokenName = new GetTokenByNameQuery(refreshToken);
        var token = await _mediator.Send(tokenName);

        if (!token.IsSuccess || token.Data is null)
            return Result.Failure(token.ErrorMessage);

        var command = new RevokeTokenCommand(token.Data.Id);

        var result = await _mediator.Send(command);

        return !result.IsSuccess 
            ? Result.Failure(result.ErrorMessage) 
            : Result.Success();
    }

    public AuthResponse GenerateTokens(UserPayload payload)
    {
        var accessToken = GenerateAccessToken(payload);
        var refreshToken = GenerateRefreshToken();

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

    private static string GenerateRefreshToken()
    {
        return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
    }
}