using CardTracker.Application.Common;
using MediatR;

namespace CardTracker.Application.Commands.UserCommands.UpdateUser;

public record UpdateVerificationStatusCommand(int UserId) : IRequest<Result<int>>;