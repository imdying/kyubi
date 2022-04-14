using System.Text;
namespace V.Components.Commands;

public static partial class Xor
{
    [Command("Xor", Description = "Read a file's content, perform xor-encryption and base64-encoding on the content or vice-versa and display the content to the output.")]
    public static void Invoke(string file, string key, bool fromBase64 = false)
    {
        byte[] fdata;
        string f_b64;
        byte[] f_xor;

        if (string.IsNullOrWhiteSpace(key))
            Internal.Error("Key cannot be empty.", true);

        if (!File.Exists(file))
            Internal.Error($"Cannot find '{file}'.", true);

        // Encoding issues.
        if (fromBase64)
            fdata = Encoding.UTF8.GetBytes(File.ReadAllText(file, Encoding.UTF8));
        else
            fdata = File.ReadAllBytes(file);

        if (fdata.Length == 0)
        {
            Internal.Warning($"Skipping because the file has no data.", true);
        }

        if (!fromBase64)
        {
            f_xor = Crytography.Xor.Encrypt(fdata, key);
            f_b64 = Convert.ToBase64String(f_xor);
        }
        else
        {
            fdata = Convert.FromBase64String(Encoding.UTF8.GetString(fdata));
            f_xor = Crytography.Xor.Encrypt(fdata, key);
            f_b64 = string.Empty;
        }

        Console.WriteLine(
            fromBase64 ? Encoding.UTF8.GetString(f_xor) : f_b64
        );
    }
}