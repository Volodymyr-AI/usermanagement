namespace UserManagement.Persistence.Auth;

public sealed class JwtOptions
{
    public string Issuer { get; init; } = default!;
    public string Audience { get; init; } = default!;
    public string Kid { get; init; } = default!;
    public string PrivateKeyPkcs8Base64 { get; init; } = default!;
}