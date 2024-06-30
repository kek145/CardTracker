using CardTracker.Application.Common;
using MediatR;

namespace CardTracker.Application.Commands.UserCommands.ResetPassword;

public record ResetPasswordCommand(int UserId, string Password) : IRequest<Result>;