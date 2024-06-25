using System.Collections.Generic;

namespace CardTracker.Infrastructure.Abstractions.Identity;

public interface IPasswordHasher
{
    void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
    bool VerifyPasswordHash(string password, IEnumerable<byte> passwordHash, byte[] passwordSalt);
}