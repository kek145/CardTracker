using CardTracker.Application.Common;
using CardTracker.Domain.Requests.Account;
using MediatR;

namespace CardTracker.Application.Commands.UserCommands.ForgotPassword;

public record ForgotPasswordCommand(ForgotPasswordRequest Request) : IRequest<Result>;