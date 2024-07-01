using MediatR;
using CardTracker.Domain.Common;

namespace CardTracker.Application.Commands.TokenCommands.RevokeToken;

public record RevokeTokenCommand(int TokenId) : IRequest<Result>;