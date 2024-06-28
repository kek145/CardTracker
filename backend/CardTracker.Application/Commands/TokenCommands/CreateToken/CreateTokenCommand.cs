using CardTracker.Application.Common;
using CardTracker.Domain.Models;
using MediatR;

namespace CardTracker.Application.Commands.TokenCommands.CreateToken;

public record CreateTokenCommand(int UserId, string RefreshToken) : IRequest<Result<int>>;