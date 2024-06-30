using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CardTracker.Domain.DTOs;
using CardTracker.Application.Common;
using CardTracker.Domain.Abstractions.Repositories;

namespace CardTracker.Application.Queries.UserQueries.GetByResetToken;

public class GetUserByResetTokenQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserByResetTokenQuery, Result<UserResetTokenDto>>
{
    private readonly IUserRepository _userRepository = userRepository;
    
    public async Task<Result<UserResetTokenDto>> Handle(GetUserByResetTokenQuery request, CancellationToken cancellationToken)
    {
        var token = await _userRepository.GetUserResetTokenAsync(request.Token, cancellationToken);

        return token is null 
            ? Result<UserResetTokenDto>.Failure("Token is null") 
            : Result<UserResetTokenDto>.Success(token);
    }
}