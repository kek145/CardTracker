using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CardTracker.Domain.DTOs;
using CardTracker.Domain.Common;
using CardTracker.Domain.Abstractions.Repositories;

namespace CardTracker.Application.Queries.UserQueries.GetByResetToken;

public class GetUserByResetTokenQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserByResetTokenQuery, Result<UserResetTokenDto>>
{
    private readonly IUserRepository _userRepository = userRepository;
    
    public async Task<Result<UserResetTokenDto>> Handle(GetUserByResetTokenQuery request, CancellationToken cancellationToken)
    {
        var token = await _userRepository.GetUserResetTokenAsync(request.Token, cancellationToken);
        
        if (token is null)
            return Result<UserResetTokenDto>.Failure("Reset token is not found!");

        return token.TokenExpires < DateTime.UtcNow 
            ? Result<UserResetTokenDto>.Failure("Reset Token expires!") 
            : Result<UserResetTokenDto>.Success(token);
    }
}