using CardTracker.Application.Common;
using CardTracker.Domain.Requests.Registration;
using MediatR;

namespace CardTracker.Application.Commands.UserCommands.AddUser;

public record AddUserCommand(RegistrationRequest Request) : IRequest<Result<int>>;