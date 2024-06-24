using CardTracker.Application.Common;
using CardTracker.Domain.Requests.Registration;
using MediatR;

namespace CardTracker.Application.Commands.UserCommands.CreateUser;

public record CreateUserCommand(RegistrationRequest Request) : IRequest<Result<int>>;