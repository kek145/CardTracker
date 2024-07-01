using MediatR;
using CardTracker.Domain.Common;
using CardTracker.Domain.Requests.Account;

namespace CardTracker.Application.Commands.UserCommands.ForgotPassword;

public record ForgotPasswordCommand(ForgotPasswordRequest Request) : IRequest<Result>;