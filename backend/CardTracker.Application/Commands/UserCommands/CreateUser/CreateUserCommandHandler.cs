using MediatR;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;
using CardTracker.Domain.Models;
using CardTracker.Application.Common;
using CardTracker.Infrastructure.Abstractions;
using CardTracker.Infrastructure.Hasher;

namespace CardTracker.Application.Commands.UserCommands.CreateUser;

public class CreateUserCommandHandler(IMapper mapper, IUserRepository userRepository) : IRequestHandler<CreateUserCommand, Result<int>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result<int>> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Request.Email, cancellationToken);

        var registration = _mapper.Map<User>(request.Request);

        if (user)
            return Result<int>.ErrorResult("User with the specified email already exists.");
        
        PasswordHasher.CreatePasswordHash(request.Request.Password, out var passwordHash, out var passwordSalt);

        registration.PasswordHash = passwordHash;
        registration.PasswordSalt = passwordSalt;

        var newUser = await _userRepository.AddUserAsync(registration, cancellationToken);

        return Result<int>.SuccessResult(newUser);

    }
}