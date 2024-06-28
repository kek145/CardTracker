using CardTracker.Application.Common;
using MediatR;

namespace CardTracker.Application.Commands.TokenCommands.RevokeToken;

public record RevokeTokenCommand(int TokenId) : IRequest<Result>;