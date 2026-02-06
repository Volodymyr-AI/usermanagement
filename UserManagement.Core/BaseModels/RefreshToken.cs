namespace UserManagement.Core.BaseModels;

public class RefreshToken
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string TokenHash { get; private set; }
    public DateTime ExpiresAtUtc { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime? RevokedAtUtc { get; private set; }
    public Guid? ReplacedByTokenId { get; private set; }
    public string? DeviceId { get; private set; }
    public string? UserAgent { get; private set; }
    public string? IpAddress { get; private set; }
    
    private RefreshToken() {}

    public static RefreshToken Issue(Guid userId, string tokenHash, DateTime utcNow, DateTime expiresAtUtc,
        string? deviceId = null, string? userAgent = null, string? ip = null)
    {
        if(userId == Guid.Empty) throw new ArgumentException("User id cannot be empty", nameof(userId));
        if(string.IsNullOrWhiteSpace(tokenHash)) throw new ArgumentException("Token hash cannot be empty", nameof(tokenHash));
        if(expiresAtUtc <= utcNow) throw new ArgumentException("Expire must be in the future", nameof(expiresAtUtc));
        
        var token = new RefreshToken()
        {
            Id = Guid.NewGuid(),
            UserId =  userId,
            TokenHash =  tokenHash,
            ExpiresAtUtc = expiresAtUtc,
            CreatedAtUtc =  utcNow,
            DeviceId =  deviceId,
            UserAgent = userAgent,
            IpAddress = ip
        };
        
        return token;
    }

    public bool IsRevoked() => RevokedAtUtc != null;
    public bool IsExpired(DateTime utcNow) => utcNow >= ExpiresAtUtc;
    public bool IsActive(DateTime utcNow) => !IsRevoked() && !IsExpired(utcNow);

    public void Revoke(DateTime utcNow, Guid? replacedByTokenId = null)
    {
        if(RevokedAtUtc != null) throw new InvalidOperationException("Token already revoked");
        if(utcNow <= CreatedAtUtc) throw new ArgumentException("Invalid revoke time", nameof(utcNow));
        
        RevokedAtUtc = utcNow;
        ReplacedByTokenId = replacedByTokenId;
    }
}