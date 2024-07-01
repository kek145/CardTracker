using MediatR;
using System.Threading;
using System.Threading.Tasks;
using CardTracker.Domain.Common;
using CardTracker.Domain.Abstractions.Repositories;
using CardTracker.Infrastructure.Abstractions.Identity;

namespace CardTracker.Application.Commands.UserCommands.ResetPassword;

public class ResetPasswordCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher) : IRequestHandler<ResetPasswordCommand, Result>
{
    private readonly IUserRepository _userRepository = userRepository;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    
    public async Task<Result> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
    {
        _passwordHasher.CreatePasswordHash(request.Password, out var passwordHash, out var passwordSalt);
        
        var password = await _userRepository.UpdatePasswordAsync(request.UserId, passwordHash, passwordSalt, cancellationToken);

        return password <= 0 
            ? Result.Failure("User not found!") 
            : Result.Success();
    }
}