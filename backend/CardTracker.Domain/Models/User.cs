using System;
using System.Collections.Generic;
using CardTracker.Domain.Abstractions;

namespace CardTracker.Domain.Models;

public class User : IEntityId<int>
{
    public int Id { get; set; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Email { get; init; } = string.Empty;
    public bool IsEmailConfirmed { get; init; }
    public byte[] PasswordHash { get; set; } = [];
    public byte[] PasswordSalt { get; set; } = [];
    public string? VerificationToken { get; set; }
    public DateTime? VerifiedAt { get; init; }
    public DateTime? VerificationTokenExpiry { get; set; }
    public string? ResetToken { get; init; }
    
    public DateTime? ResetTokenExpiry { get; init; }
    public virtual ICollection<RefreshToken> RefreshToken { get; init; } = [];
}