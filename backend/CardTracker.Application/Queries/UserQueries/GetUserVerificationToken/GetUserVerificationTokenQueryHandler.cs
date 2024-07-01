using System;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CardTracker.Domain.DTOs;
using CardTracker.Domain.Common;
using CardTracker.Domain.Abstractions.Repositories;

namespace CardTracker.Application.Queries.UserQueries.GetUserVerificationToken;

public class GetUserVerificationTokenQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserVerificationTokenQuery, Result<UserVerificationDto>>
{
    private readonly IUserRepository _userRepository = userRepository;
    
    public async Task<Result<UserVerificationDto>> Handle(GetUserVerificationTokenQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetVerificationTokenAsync(request.Request.Token, cancellationToken);

        if (user is null)
            return Result<UserVerificationDto>.Failure("Verification token is not found!");

        return user.VerificationTokenExpiry < DateTime.UtcNow 
            ? Result<UserVerificationDto>.Failure("Verification token expires!") 
            : Result<UserVerificationDto>.Success(user);
    }
}