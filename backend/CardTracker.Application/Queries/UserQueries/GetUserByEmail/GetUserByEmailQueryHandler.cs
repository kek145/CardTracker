using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CardTracker.Domain.Models;
using CardTracker.Domain.Common;
using CardTracker.Domain.Abstractions.Repositories;

namespace CardTracker.Application.Queries.UserQueries.GetUserByEmail;

public class GetUserByEmailQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserByEmailQuery, Result<User>>
{
    private readonly IUserRepository _userRepository = userRepository;
    
    public async Task<Result<User>> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Email, cancellationToken);

        return user is null ?
            Result<User>.Failure($"A user with this email: {request.Email} will not be found in the system") 
            : Result<User>.Success(user);
    }
}