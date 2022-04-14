using System.Text;
using System.Security.Cryptography;

namespace V.Components.Crytography;

public class Sha256
{
    public static string GetHash(string key)
    {
        using (var hash = SHA256.Create())
        {
            var hashBuffer = hash.ComputeHash(
                Encoding.UTF8.GetBytes(
                    key
                )
            );

            return BitConverter.ToString(hashBuffer)
                               .Replace("-", null)
                               .ToLower();
        }
    }

    public static byte[] GetByteHash(string key)
    {
        using (var hash = SHA256.Create())
        {
            return hash.ComputeHash(
                Encoding.UTF8.GetBytes(
                    key
                )
            );
        }
    }
}