using System;

namespace CardTracker.Application.Common.Helpers;

public static class TokenGenerator
{
    public static string GenerateVerificationToken()
    {
        return Guid.NewGuid().ToString("N");
    }
}