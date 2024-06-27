using CardTracker.Application.Common;
using CardTracker.Domain.Models;
using MediatR;

namespace CardTracker.Application.Commands.RefreshTokenCommands.CreateRefreshToken;

public record CreateRefreshTokenCommand(int UserId, string RefreshToken) : IRequest<Result<int>>;