using MediatR;
using CardTracker.Domain.Common;
using CardTracker.Domain.Requests.Registration;

namespace CardTracker.Application.Commands.UserCommands.AddUser;

public record AddUserCommand(RegistrationRequest Request) : IRequest<Result<int>>;