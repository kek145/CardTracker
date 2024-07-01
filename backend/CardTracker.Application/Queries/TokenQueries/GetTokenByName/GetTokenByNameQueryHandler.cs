using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CardTracker.Domain.Models;
using CardTracker.Domain.Common;
using CardTracker.Domain.Abstractions.Repositories;

namespace CardTracker.Application.Queries.TokenQueries.GetTokenByName;

public class GetTokenByNameQueryHandler(ITokenRepository tokenRepository) : IRequestHandler<GetTokenByNameQuery, Result<RefreshToken>>
{
    private readonly ITokenRepository _tokenRepository = tokenRepository;
    
    public async Task<Result<RefreshToken>> Handle(GetTokenByNameQuery request, CancellationToken cancellationToken)
    {
        var token = await _tokenRepository.GetRefreshTokenByNameAsync(request.RefreshToken, cancellationToken);

        if (token is null)
            return Result<RefreshToken>.Failure("Token is null!");

        if (token.ExpiresAt < DateTime.UtcNow)
            return Result<RefreshToken>.Failure("Refresh token has expired!");

        return token.IsRevoked 
            ? Result<RefreshToken>.Failure("Refresh token has been revoked!") 
            : Result<RefreshToken>.Success(token);
    }
}