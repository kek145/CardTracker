using System.Text;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using CardTracker.Infrastructure.Abstractions.Identity;

namespace CardTracker.Infrastructure.Hasher;

public class PasswordHasher : IPasswordHasher
{
    public void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512();
        
        passwordSalt = hmac.Key;
        
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }

    public bool VerifyPasswordHash(string password, IEnumerable<byte> passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        
        var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        
        return computeHash.SequenceEqual(passwordHash);
    }
}