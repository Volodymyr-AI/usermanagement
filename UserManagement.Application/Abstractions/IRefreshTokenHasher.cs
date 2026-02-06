namespace UserManagement.Application.Abstractions;

public interface IRefreshTokenHasher
{
    string Hash(string refreshToken);
}