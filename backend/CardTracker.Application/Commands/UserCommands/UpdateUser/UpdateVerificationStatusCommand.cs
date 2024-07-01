using MediatR;
using CardTracker.Domain.Common;

namespace CardTracker.Application.Commands.UserCommands.UpdateUser;

public record UpdateVerificationStatusCommand(int UserId) : IRequest<Result<int>>;