using UserManagement.Core.ValueObjects;
using UserManagement.Core.ValueTypes;

namespace UserManagement.Core.BaseModels;

public class User
{
    public Guid Id { get; private set; }
    public Email Email { get; private set; }
    public string PasswordHash { get; private set; }
    public UserStatus Status { get; private set; }
    public bool EmailConfirmed { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    
    private readonly List<UserRole> _roles = new();
    public IReadOnlyCollection<UserRole> Roles => _roles;
    
    private User(){}

    public static User Create(Email email, string passwordHash, DateTime utcNow)
    {
        if(string.IsNullOrWhiteSpace(passwordHash)) throw new ArgumentException("Password cannot be empty", nameof(passwordHash));
        if(email is null) throw new ArgumentException("Email cannot be null", nameof(email));

        var user = new User()
        {
            Id = Guid.NewGuid(),
            Email = email,
            PasswordHash = passwordHash,
            Status = UserStatus.Active,
            EmailConfirmed = false,
            CreatedAtUtc = utcNow
        };
        user._roles.Add(UserRole.User);
        
        return user;
    }

    public void GrantAdmin()
    {
        if(Status != UserStatus.Active) throw new InvalidOperationException("Cannot set admin role for the deactivated account");
        if(_roles.Contains(UserRole.Admin)) throw new  InvalidOperationException("Cannot add admin role for this account");
        
        _roles.Add(UserRole.Admin);
    }

    public void Disable()
    {
        if(Status == UserStatus.Disabled) throw new InvalidOperationException("Cannot disable already deactivated account");
        
        Status =  UserStatus.Disabled;
    }

    public void ConfirmEmail()
    {
        if(Status == UserStatus.Disabled) throw new InvalidOperationException("Cannot confirm email for the deactivated account");
        EmailConfirmed = true;
    }
}