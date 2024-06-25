using MediatR;
using CardTracker.Domain.Models;
using CardTracker.Application.Common;

namespace CardTracker.Application.Queries.UserQueries.GetUserByEmail;

public record GetUserByEmailQuery(string Email) : IRequest<Result<User>>;