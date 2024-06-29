﻿using System.Threading.Tasks;
using CardTracker.Domain.Abstractions;
using CardTracker.Domain.Requests.Account;

namespace CardTracker.Application.Services.AccountService;

public interface IAccountService
{
    Task<IBaseResponse<string>> VerifyAccountAsync(VerificationTokenRequest request);
}