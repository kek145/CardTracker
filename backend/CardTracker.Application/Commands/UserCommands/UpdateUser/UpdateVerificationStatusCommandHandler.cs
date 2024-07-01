using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CardTracker.Domain.Common;
using CardTracker.Domain.Abstractions.Repositories;

namespace CardTracker.Application.Commands.UserCommands.UpdateUser;

public class UpdateVerificationStatusCommandHandler(IUserRepository userRepository) : IRequestHandler<UpdateVerificationStatusCommand, Result<int>>
{
    private readonly IUserRepository _userRepository = userRepository;
    
    public async Task<Result<int>> Handle(UpdateVerificationStatusCommand request, CancellationToken cancellationToken)
    {
        var result = await _userRepository.UpdateVerificationStatusAsync(request.UserId, cancellationToken);

        return result <= 0 
            ? Result<int>.Failure("Error updating verification status.") 
            : Result<int>.Success(result);
    }
}