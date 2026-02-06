namespace UserManagement.Persistence.Token;

public sealed class RefreshTokenOptions
{
    public string HmacKey { get; init; } = default!;
}