using System;
using CardTracker.Domain.Abstractions;

namespace CardTracker.Domain.Models;

public class RefreshToken : IEntityId<int>
{
    public int Id { get; set; }
    public string Token { get; set; } = string.Empty;
    public bool IsRevoked { get; set; }
    public DateTime ExpiresAt { get; set; }
    public int UserId { get; set; }
    public User User { get; set; } = new();
}