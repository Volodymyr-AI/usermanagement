using System.Security.Cryptography;
using UserManagement.Persistence.Auth.DTO;

namespace UserManagement.Persistence.Auth.Abstractions;

public interface IRsaKeyService
{
    string Kid { get; }
    RSA GetPrivateKey();
    IReadOnlyCollection<JwkDto> GetPublicJwks();
}