using CardTracker.Application.Common;
using CardTracker.Domain.DTOs;
using CardTracker.Domain.Models;
using MediatR;

namespace CardTracker.Application.Queries.UserQueries.GetByResetToken;

public record GetUserByResetTokenQuery(string Token) : IRequest<Result<UserResetTokenDto>>;