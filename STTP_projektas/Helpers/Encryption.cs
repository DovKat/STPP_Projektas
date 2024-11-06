using System.Security.Cryptography;
using System.Text;

namespace STTP_projektas.Helpers;

public static class Encryption
{
    public static string ToSHA256(this string input)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(input);
        var hashBytes = sha256.ComputeHash(bytes);
        return Convert.ToBase64String(hashBytes);
    }
}