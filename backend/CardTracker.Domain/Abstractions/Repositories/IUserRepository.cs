﻿using System.Threading;
using System.Threading.Tasks;
using CardTracker.Domain.DTOs;
using CardTracker.Domain.Models;

namespace CardTracker.Domain.Abstractions.Repositories;

public interface IUserRepository
{
    Task<int> AddUserAsync(User user, CancellationToken ct = default);
    Task<User?> GetUserByIdAsync(int userId, CancellationToken ct = default);
    Task<User?> GetUserByEmailAsync(string email, CancellationToken ct = default);
    Task<int> UpdateVerificationStatusAsync(int userId, CancellationToken ct = default);
    Task<UserVerificationDto?> GetVerificationTokenAsync(string token, CancellationToken ct = default);
}