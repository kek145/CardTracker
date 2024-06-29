using MediatR;
using CardTracker.Domain.DTOs;
using CardTracker.Application.Common;
using CardTracker.Domain.Requests.Account;

namespace CardTracker.Application.Queries.UserQueries.GetUserVerificationToken;

public record GetUserVerificationTokenQuery(VerificationTokenRequest Request) : IRequest<Result<UserVerificationDto>>;