using System.Buffers.Text;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;
using UserManagement.Application.Abstractions;
using UserManagement.Persistence.Auth.Abstractions;
using UserManagement.Persistence.Auth.DTO;

namespace UserManagement.Persistence.Auth;

public sealed class RsaKeyService : IRsaKeyService, IDisposable
{
    private readonly RSA _rsa;
    public string Kid { get; }

    public RsaKeyService(IOptions<JwtOptions> opt)
    {
        var o = opt.Value;
        if (string.IsNullOrWhiteSpace(o.Kid)) throw new ArgumentException("Kid value is missing", nameof(opt.Value));
        Kid = o.Kid;
        var keyBytes = Convert.FromBase64String(o.PrivateKeyPkcs8Base64);
        if (string.IsNullOrWhiteSpace(o.PrivateKeyPkcs8Base64)) throw new ArgumentException("Pkc s8 private key value is missing", nameof(o.PrivateKeyPkcs8Base64));
        _rsa = RSA.Create();
        _rsa.ImportPkcs8PrivateKey(keyBytes, out _);
    }

    public IReadOnlyCollection<JwkDto> GetPublicJwks()
    {
        var p = _rsa.ExportParameters(false);

        if (p.Modulus is null || p.Exponent is null)
            throw new InvalidOperationException("RSA public parameters are missing");

        var n = Base64UrlEncode(p.Modulus);
        var e = Base64UrlEncode(p.Exponent);
        
        JwkDto jwk = new JwkDto()
        {
            Kty = "RSA",
            Use = "sig",
            Alg = "RS256",
            Kid = Kid,
            N = n,
            E = e 
        };

        return new List<JwkDto> { jwk };
    }
    
    public RSA GetPrivateKey() => _rsa;

    private static string Base64UrlEncode(byte[] bytes)
        => Convert.ToBase64String(bytes)
            .TrimEnd('=')
            .Replace('+', '-')
            .Replace('/', '_');
    
    public void Dispose() => _rsa.Dispose();
}