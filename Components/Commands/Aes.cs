using System.Text;
using Security = V.Components.Crytography;
namespace V.Components.Commands;

public static class Aes
{
    [Command("Aes", Description = "Perform aes-encryption and return the result as a base64-encoded string or vice-versa." +
                                  "The command can also handle 2 types of input, a string or a file-path which's content is then read and processed.")]
    public static void Invoke(string Key,
                              string FileOrString,
                              bool ToEncrypt = true,
                              bool IsFile = true)
    {
        dynamic Content;

        if (string.IsNullOrWhiteSpace(Key))
            Internal.Error("The key cannot be empty.", true);

        // Working with string.
        if (!IsFile && string.IsNullOrWhiteSpace(FileOrString))
            Internal.Error("The string cannot be null or empty.", true);

        if (IsFile && !File.Exists(FileOrString))
            throw new FileNotFoundException(FileOrString);

        // File content or string
        Content = IsFile ? ReadFileSafely(FileOrString) : FileOrString;

        if (IsFile && Content.Length == 0)
            Internal.Warning($"Skipping because the file contains no data.", true);

        var _key = Security.Sha256.GetByteHash(Key);
        var _iv = Security.BootlegHash.GetHash(Encoding.UTF8.GetString(_key));

        Console.WriteLine(ToEncrypt ? EncryptContent(Content, _key, _iv) : DecryptContent(Content, _key, _iv));
    }

    private static string EncryptContent(string content,
                                         byte[] key,
                                         byte[] iv) => Convert.ToBase64String(Security.Aes.EncryptStringToBytes(content, key, iv));

    private static string DecryptContent(string content,
                                         byte[] key,
                                         byte[] iv) => Security.Aes.DecryptStringFromBytes(Convert.FromBase64String(content), key, iv);

    private static string ReadFileSafely(string fileName)
    {
        string output = string.Empty;
        using (var fs = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
        using (var sr = new StreamReader(fs, Encoding.UTF8))
        {
            //Continue to read until you reach end of file
            while (!sr.EndOfStream)
            {
                output += (char)sr.Read();
            }
        }
        return output;
    }
}