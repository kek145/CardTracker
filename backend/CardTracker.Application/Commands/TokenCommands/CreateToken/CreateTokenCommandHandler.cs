using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CardTracker.Domain.Models;
using CardTracker.Application.Common;
using CardTracker.Domain.Abstractions.Repositories;

namespace CardTracker.Application.Commands.TokenCommands.CreateToken;

public class CreateTokenCommandHandler(ITokenRepository tokenRepository) 
    : IRequestHandler<CreateTokenCommand, Result<int>>
{
    private readonly ITokenRepository _tokenRepository = tokenRepository;
    
    public async Task<Result<int>> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
    {
        var token = await _tokenRepository.GetRefreshTokenByUserIdAsync(request.UserId, cancellationToken);

        if (token is not null)
        {
            if (token.ExpiresAt < DateTime.UtcNow || token.IsRevoked)
            {
                await _tokenRepository.DeleteRefreshTokenAsync(token.Id, cancellationToken);
            }
            else
            {
                await _tokenRepository.UpdateRefreshTokenAsync(token.Id, request.RefreshToken, cancellationToken);
                return Result<int>.Success(token.Id);
            }
        }
        
        var refreshToken = new RefreshToken
        {
            Token = request.RefreshToken,
            IsRevoked = false,
            ExpiresAt = DateTime.UtcNow.AddMinutes(3),
            UserId = request.UserId
        };

        var result = await _tokenRepository.AddRefreshTokenAsync(refreshToken, cancellationToken);
        
        return Result<int>.Success(result);
    }
}