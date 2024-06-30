using System;
using System.Security.Cryptography;

namespace CardTracker.Application.Common.Helpers;

public static class TokenGenerator
{
    public static string GenerateVerificationToken()
    {
        return Convert.ToHexString(RandomNumberGenerator.GetBytes(64));
    }
}