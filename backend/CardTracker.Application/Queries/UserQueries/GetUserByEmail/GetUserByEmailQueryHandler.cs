using System.Threading;
using System.Threading.Tasks;
using CardTracker.Application.Common;
using CardTracker.Domain.Models;
using CardTracker.Infrastructure.Abstractions.Repositories;
using MediatR;

namespace CardTracker.Application.Queries.UserQueries.GetUserByEmail;

public class GetUserByEmailQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserByEmailQuery, Result<User>>
{
    private readonly IUserRepository _userRepository = userRepository;
    
    public async Task<Result<User>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email, cancellationToken);

        return user is null ?
            Result<User>.ErrorResult($"A user with this email: {request.Email} will not be found in the system") 
            : Result<User>.SuccessResult(user);
    }
}