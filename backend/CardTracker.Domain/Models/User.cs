using System;
using CardTracker.Domain.Abstractions;

namespace CardTracker.Domain.Models;

public class User : IEntityId<int>
{
    public int Id { get; set; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string SurName { get; init; } = string.Empty;
    public string Email = string.Empty;
    public bool EmailConfirmed { get; init; }
    public byte[] PasswordHash { get; init; } = [];
    public byte[] PasswordSalt { get; init; } = [];
    public string? ResetToken { get; set; }
    public DateTime? ResetTokenExpiry { get; init; }
}