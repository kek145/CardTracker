using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CardTracker.Application.Common;
using CardTracker.Domain.Abstractions.Repositories;

namespace CardTracker.Application.Commands.UserCommands.ForgotPassword;

public class ForgotPasswordCommandHandler(IUserRepository userRepository) : IRequestHandler<ForgotPasswordCommand, Result>
{
    private readonly IUserRepository _userRepository = userRepository;
    
    public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var token = await _userRepository.UpdateForgotPasswordTokenAsync(request.Request.Email, cancellationToken);

        return token <= 0 
            ? Result.Failure("User not found!") 
            : Result.Success();
    }
}