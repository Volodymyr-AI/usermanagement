using System.Security.Cryptography;
using System.Text;
using UserManagement.Application.Abstractions;

namespace UserManagement.Persistence.Token;

public sealed class HmacRefreshTokenHasher : IRefreshTokenHasher
{
    private readonly byte[] _key;

    public HmacRefreshTokenHasher(string base64Key)
    {
        if(string.IsNullOrWhiteSpace(base64Key))
            throw new ArgumentException("HMAC Key is missing", nameof(base64Key));
        
        _key = Convert.FromBase64String(base64Key);
        if(_key.Length < 32)
            throw new ArgumentException("HMAC key must be at least 32 bytes", nameof(base64Key));
    }
    
    public string Hash(string refreshToken)
    {
        if(string.IsNullOrWhiteSpace(refreshToken))
            throw new ArgumentException("Refresh token cannot be empty", nameof(refreshToken));
        
        using var hmac = new HMACSHA256(_key);
        var bytes = Encoding.UTF8.GetBytes(refreshToken);
        var hash = hmac.ComputeHash(bytes);
        
        return ToHexLower(hash);
    }

    private static string ToHexLower(byte[] bytes)
    {
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}