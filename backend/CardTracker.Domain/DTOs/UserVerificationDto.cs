using System;

namespace CardTracker.Domain.DTOs;

public record UserVerificationDto(
    int UserId,
    bool IsEmailConfirmed,
    string? Token,
    DateTime? VerifiedAt,
    DateTime? VerificationTokenExpiry);