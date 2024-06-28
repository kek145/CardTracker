using CardTracker.Application.Common;
using CardTracker.Domain.Models;
using MediatR;

namespace CardTracker.Application.Queries.UserQueries.GetUserById;

public record GetUserByIdQuery(int UserId) : IRequest<Result<User>>;