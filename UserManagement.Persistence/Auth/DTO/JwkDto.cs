namespace UserManagement.Persistence.Auth.DTO;

public sealed record JwkDto
{
    public string Kty { get; init; } = default!;
    public string Use { get; init; } = default!;
    public string Alg { get; init; } = default!;
    public string Kid { get; init; } = default!;
    public string N { get; init; } = default!;
    public string E { get; init; } = default!;
}