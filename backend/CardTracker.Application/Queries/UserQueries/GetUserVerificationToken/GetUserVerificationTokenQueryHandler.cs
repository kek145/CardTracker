using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CardTracker.Domain.DTOs;
using CardTracker.Application.Common;
using CardTracker.Domain.Abstractions.Repositories;

namespace CardTracker.Application.Queries.UserQueries.GetUserVerificationToken;

public class GetUserVerificationTokenQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserVerificationTokenQuery, Result<UserVerificationDto>>
{
    private readonly IUserRepository _userRepository = userRepository;
    
    public async Task<Result<UserVerificationDto>> Handle(GetUserVerificationTokenQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetVerificationTokenAsync(request.Request.Token, cancellationToken);

        return user is null 
            ? Result<UserVerificationDto>.Failure("Invalid token!") 
            : Result<UserVerificationDto>.Success(user);
    }
}