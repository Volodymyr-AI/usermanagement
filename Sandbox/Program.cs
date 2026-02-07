using System.Security.Cryptography;

namespace Sandbox;

class Program
{
    static void Main(string[] args)
    {
        //byte[] bytes = new byte[32];
        //using var hmac = new HMACSHA512(bytes);
        //RandomNumberGenerator.Fill(bytes);
        //Console.WriteLine(Convert.ToHexString(bytes).ToLowerInvariant());

        using var rsa = RSA.Create(2048);
        var pkcs8Bytes = rsa.ExportPkcs8PrivateKey();
        var base64 = Convert.ToBase64String(pkcs8Bytes);
        Console.WriteLine("=== PrivateKeyPkcs8Base64 ===");
        Console.WriteLine(base64);
        Console.WriteLine();
        
        var publicParams = rsa.ExportParameters(false);
        Console.WriteLine($"Modulus size: {publicParams.Modulus!.Length * 8} bits");

        try
        {
            rsa.ImportPkcs8PrivateKey(
                Convert.FromBase64String(base64),
                out var _);
            Console.WriteLine("Key is valid");
        }
        catch (CryptographicException e)
        {
            Console.WriteLine(e.Message);
        }
    }
}