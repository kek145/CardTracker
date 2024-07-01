using MediatR;
using CardTracker.Domain.Models;
using CardTracker.Domain.Common;

namespace CardTracker.Application.Queries.TokenQueries.GetTokenByName;

public record GetTokenByNameQuery(string RefreshToken) : IRequest<Result<RefreshToken>>;