using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UserManagement.Core.ValueObjects;

public sealed record Email(string Value)
{
    public static Email From(string input)
    {
        string email = NormalizeEmail(input);
        return new Email(email);
    }

    private static string NormalizeEmail(string rawEmail)
    {
        if(string.IsNullOrEmpty(rawEmail))
            throw new ArgumentException("Email cannot be null or empty");
        
        rawEmail = rawEmail.Trim().ToLowerInvariant();
        var parts = rawEmail.Split("@", 2);
        
        if (parts.Length != 2)
            throw new ArgumentException("Invalid email format");
        
        string localPart = parts[0];
        string domain = parts[1];
        
        if (string.IsNullOrWhiteSpace(localPart) || string.IsNullOrWhiteSpace(domain))
            throw new ArgumentException("Invalid email name or domain format");

        return $"{localPart}@{domain}";
    }
}