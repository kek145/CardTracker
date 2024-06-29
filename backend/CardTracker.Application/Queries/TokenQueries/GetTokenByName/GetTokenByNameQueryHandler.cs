using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CardTracker.Domain.Models;
using CardTracker.Application.Common;
using CardTracker.Domain.Abstractions.Repositories;

namespace CardTracker.Application.Queries.TokenQueries.GetTokenByName;

public class GetTokenByNameQueryHandler(ITokenRepository tokenRepository) : IRequestHandler<GetTokenByNameQuery, Result<RefreshToken>>
{
    private readonly ITokenRepository _tokenRepository = tokenRepository;
    
    public async Task<Result<RefreshToken>> Handle(GetTokenByNameQuery request, CancellationToken cancellationToken)
    {
        Console.WriteLine(request.RefreshToken);
        
        var token = await _tokenRepository.GetRefreshTokenByNameAsync(request.RefreshToken, cancellationToken);
        
        Console.WriteLine(token);

        return token is null 
            ? Result<RefreshToken>.Failure("RefreshToken is null") 
            : Result<RefreshToken>.Success(token);
    }
}