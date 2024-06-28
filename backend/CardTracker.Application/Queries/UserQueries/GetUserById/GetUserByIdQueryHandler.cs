using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CardTracker.Domain.Models;
using CardTracker.Application.Common;
using CardTracker.Infrastructure.Abstractions.Repositories;

namespace CardTracker.Application.Queries.UserQueries.GetUserById;

public class GetUserByIdQueryHandler(IUserRepository userRepository) : IRequestHandler<GetUserByIdQuery, Result<User>>
{
    private readonly IUserRepository _userRepository = userRepository;
    
    public async Task<Result<User>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByIdAsync(request.UserId, cancellationToken);

        return user is null 
            ? Result<User>.Failure("User not found!") 
            : Result<User>.Success(user);
    }
}