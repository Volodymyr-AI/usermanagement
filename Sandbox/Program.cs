using System.Security.Cryptography;

namespace Sandbox;

class Program
{
    static void Main(string[] args)
    {
        byte[] bytes = new byte[32];
        using var hmac = new HMACSHA512(bytes);
        RandomNumberGenerator.Fill(bytes);
        Console.WriteLine(Convert.ToHexString(bytes).ToLowerInvariant());
    }
}