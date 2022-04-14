using System.Text;
using Security = V.Components.Crytography;
namespace V.Components.Commands;

public static class Aes
{
    [Command("Aes", Description = "Perform aes-encryption on a file's content and return the computed data in base64-encoding or vice-versa.")]
    public static void Invoke(string file, string key, bool fromBase64 = false, bool toBase64 = true)
    {
        List<object> fdata = new List<object>();

        if (string.IsNullOrWhiteSpace(key))
            Internal.Error("Key cannot be empty.", true);

        if (!File.Exists(file))
            Internal.Error($"Cannot find the file '{file}'.", true);

        // Encoding issues.
        if (fromBase64)
            fdata.Add(File.ReadAllText(file, Encoding.UTF8));
        else
            fdata.Add(File.ReadAllBytes(file));

        if (fdata.Count == 0)
        {
            Internal.Warning($"Skipping because the file has no data.", true);
            throw new Exception();
        }

        var _key = Security.Sha256.GetByteHash(key);
        var _iv = Security.BootlegHash.GetHash(Encoding.UTF8.GetString(_key));

        // The file was encrypted and
        // needs to convert back to string.
        if (fromBase64)
        {
            Console.WriteLine(
                Security.Aes.Decrypt(
                    (string)fdata[0],
                    _key,
                    _iv
                )
            );
            return;
        }

        // Needs to be encrypted or/and encoded.
        if (toBase64 && !fromBase64)
        {
            Console.WriteLine(
                Security.Aes.EncryptAndEncode(
                    BitConverter.ToString((byte[])fdata[0]), 
                    _key, 
                    _iv
                )
            );
            return;
        }
    }
}