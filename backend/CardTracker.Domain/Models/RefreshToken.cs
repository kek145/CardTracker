using System;
using CardTracker.Domain.Abstractions;

namespace CardTracker.Domain.Models;

public class RefreshToken : IEntityId<int>
{
    public int Id { get; set; }
    public string Token { get; init; } = string.Empty;
    public bool IsRevoked { get; init; }
    public DateTime? RevokedAt { get; init; }
    public DateTime ExpiresAt { get; init; }
    public int UserId { get; init; }
    public User User { get; init; } = null!;
}