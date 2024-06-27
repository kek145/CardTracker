using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CardTracker.Domain.Models;
using CardTracker.Application.Common;
using CardTracker.Infrastructure.Abstractions.Repositories;

namespace CardTracker.Application.Commands.RefreshTokenCommands.CreateRefreshToken;

public class CreateRefreshTokenCommandHandler(IRefreshTokenRepository refreshTokenRepository) 
    : IRequestHandler<CreateRefreshTokenCommand, Result<int>>
{
    private readonly IRefreshTokenRepository _refreshTokenRepository = refreshTokenRepository;
    
    public async Task<Result<int>> Handle(CreateRefreshTokenCommand request, CancellationToken cancellationToken)
    {
        var refreshToken = new RefreshToken
        {
            Token = request.RefreshToken,
            IsRevoked = false,
            ExpiresAt = DateTime.UtcNow.AddDays(30),
            UserId = request.UserId
        };

        var result = await _refreshTokenRepository.AddRefreshTokenAsync(refreshToken, cancellationToken);

        return result <= 0 
            ? Result<int>.ErrorResult("An error occurred while generating the refresh token") 
            : Result<int>.SuccessResult(result);
    }
}