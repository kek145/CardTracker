﻿using MediatR;
using CardTracker.Domain.DTOs;
using CardTracker.Domain.Common;

namespace CardTracker.Application.Queries.UserQueries.GetByResetToken;

public record GetUserByResetTokenQuery(string Token) : IRequest<Result<UserResetTokenDto>>;