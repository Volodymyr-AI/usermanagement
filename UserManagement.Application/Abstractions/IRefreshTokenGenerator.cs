namespace UserManagement.Application.Abstractions;

public interface IRefreshTokenGenerator
{
    string Generate();
}