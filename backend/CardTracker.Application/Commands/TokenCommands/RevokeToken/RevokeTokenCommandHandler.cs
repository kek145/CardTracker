using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CardTracker.Application.Common;
using CardTracker.Infrastructure.Abstractions.Repositories;

namespace CardTracker.Application.Commands.TokenCommands.RevokeToken;

public class RevokeTokenCommandHandler(ITokenRepository tokenRepository) : IRequestHandler<RevokeTokenCommand, Result>
{
    private readonly ITokenRepository _tokenRepository = tokenRepository;
    
    public async Task<Result> Handle(RevokeTokenCommand request, CancellationToken cancellationToken)
    {
        var token = await _tokenRepository.RevokeRefreshTokenAsync(request.TokenId, cancellationToken);

        return token <= 0 
            ? Result.Failure("An error occurred while revoking the refresh token!") 
            : Result.Success();
    }
}