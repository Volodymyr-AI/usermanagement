using System.Security.Cryptography;
using UserManagement.Application.Abstractions;

namespace UserManagement.Persistence.Token;

public sealed class CryptoRefreshTokenGenerator : IRefreshTokenGenerator
{
    public string Generate()
    {
        var tokenBytes = new byte[32];
        RandomNumberGenerator.Fill(tokenBytes);
        
        return Base64UrlEncode(tokenBytes);
    }

    private static string Base64UrlEncode(byte[] bytes)
    {
        if(bytes.Length < 32)
            throw new ArgumentException("Token entropy too low", nameof(bytes));
        var encodedStr = Convert.ToBase64String(bytes).TrimEnd('=').Replace('+', '-').Replace('/', '_');
        return encodedStr;
    }
}