using UserManagement.Persistence.Auth.DTO;

namespace UserManagement.WebAPI.DTO;

public sealed record JwksResponse(IReadOnlyCollection<JwkDto> Keys);