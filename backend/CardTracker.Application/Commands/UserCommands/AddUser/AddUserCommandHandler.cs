using System;
using MediatR;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;
using CardTracker.Domain.Models;
using CardTracker.Application.Common;
using CardTracker.Application.Common.Helpers;
using CardTracker.Infrastructure.Abstractions.Identity;
using CardTracker.Domain.Abstractions.Repositories;

namespace CardTracker.Application.Commands.UserCommands.AddUser;

public class AddUserCommandHandler(IMapper mapper, IPasswordHasher passwordHasher, IUserRepository userRepository) : IRequestHandler<AddUserCommand, Result<int>>
{
    private readonly IMapper _mapper = mapper;
    private readonly IPasswordHasher _passwordHasher = passwordHasher;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result<int>> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetUserByEmailAsync(request.Request.Email, cancellationToken);

        var registration = _mapper.Map<User>(request.Request);

        if (user is not null)
            return Result<int>.Failure("User with the specified email already exists.");
        
        _passwordHasher.CreatePasswordHash(request.Request.Password, out var passwordHash, out var passwordSalt);

        registration.PasswordHash = passwordHash;
        registration.PasswordSalt = passwordSalt;
        registration.VerificationToken = TokenGenerator.GenerateVerificationToken();
        registration.VerificationTokenExpiry = DateTime.UtcNow.AddHours(24);

        var newUser = await _userRepository.AddUserAsync(registration, cancellationToken);

        return Result<int>.Success(newUser);

    }
}