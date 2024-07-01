using MediatR;
using CardTracker.Domain.Common;

namespace CardTracker.Application.Commands.UserCommands.ResetPassword;

public record ResetPasswordCommand(int UserId, string Password) : IRequest<Result>;