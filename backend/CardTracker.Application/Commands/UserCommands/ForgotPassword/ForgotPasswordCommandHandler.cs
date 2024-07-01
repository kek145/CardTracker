using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CardTracker.Application.Common.Helpers;
using CardTracker.Domain.Common;
using CardTracker.Domain.Abstractions.Repositories;

namespace CardTracker.Application.Commands.UserCommands.ForgotPassword;

public class ForgotPasswordCommandHandler(IUserRepository userRepository) : IRequestHandler<ForgotPasswordCommand, Result>
{
    private readonly IUserRepository _userRepository = userRepository;
    
    public async Task<Result> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        var token = TokenGenerator.GenerateVerificationToken();
        
        var result = await _userRepository.UpdateForgotPasswordTokenAsync(request.Request.Email, token,  cancellationToken);

        return result <= 0 
            ? Result.Failure("User not found!") 
            : Result.Success();
    }
}