using MediatR;
using CardTracker.Domain.Models;
using CardTracker.Domain.Common;

namespace CardTracker.Application.Queries.UserQueries.GetUserByEmail;

public record GetUserByEmailQuery(string Email) : IRequest<Result<User>>;