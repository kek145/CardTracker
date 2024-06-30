using System;

namespace CardTracker.Domain.DTOs;

public record UserResetTokenDto(int UserId, string? ResetToken, DateTime? TokenExpires);