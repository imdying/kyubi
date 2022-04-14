using System.Text;
namespace V.Components.Crytography;

public class Xor
{
    public static byte[] Encrypt(byte[] input, string key)
    {
        var Keys = Encoding.UTF8.GetBytes(key);
        for (int i = 0; i < input.Length; i++)
            input[i] = (byte)(input[i] ^ Keys[i % Keys.Length]);
        return input;
    }
}