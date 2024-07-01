using MediatR;
using CardTracker.Domain.Common;

namespace CardTracker.Application.Commands.TokenCommands.CreateToken;

public record CreateTokenCommand(int UserId, string RefreshToken) : IRequest<Result<int>>;