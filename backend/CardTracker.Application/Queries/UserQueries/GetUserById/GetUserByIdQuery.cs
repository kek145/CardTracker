using MediatR;
using CardTracker.Domain.Models;
using CardTracker.Domain.Common;

namespace CardTracker.Application.Queries.UserQueries.GetUserById;

public record GetUserByIdQuery(int UserId) : IRequest<Result<User>>;