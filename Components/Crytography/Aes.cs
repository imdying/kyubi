
using System.Security.Cryptography;
using System.Text;
using Crypt = System.Security.Cryptography;

namespace V.Components.Crytography;

public static class Aes
{
    public static byte[] Encrypt(string plainText, byte[] Key, byte[] IV)
    {
        if (plainText == null || plainText.Length <= 0 ||
            Key == null || Key.Length <= 0 ||
            IV == null || IV.Length <= 0)
            throw new ArgumentNullException();

        byte[] encrypted;

        // Create an Aes object
        // with the specified key and IV.
        using (var aesAlg = Crypt.Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for encryption.
            using (MemoryStream msEncrypt = new MemoryStream())
            {
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {
                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                    }
                    encrypted = msEncrypt.ToArray();
                }
            }
        }

        // Return the encrypted bytes from the memory stream.
        return encrypted;
    }

    public static string Decrypt(byte[] cipherText, byte[] Key, byte[] IV)
    {
        if (cipherText == null || cipherText.Length <= 0 ||
            Key == null || Key.Length <= 0 ||
            IV == null || IV.Length <= 0)
            throw new ArgumentNullException();

        // Declare the string used to hold
        // the decrypted text.
        string plaintext;

        // Create an Aes object
        // with the specified key and IV.
        using (var aesAlg = Crypt.Aes.Create())
        {
            aesAlg.Key = Key;
            aesAlg.IV = IV;

            // Create a decryptor to perform the stream transform.
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            // Create the streams used for decryption.
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {

                        // Read the decrypted bytes from the decrypting stream
                        // and place them in a string.
                        plaintext = srDecrypt.ReadToEnd();
                    }
                }
            }
        }
        return plaintext;
    }

    /// <summary>
    /// Encrypt and encode to base64
    /// </summary>
    public static string EncryptAndEncode(string plainText, byte[] Key, byte[] IV)
    {
        return Convert.ToBase64String(Encrypt(plainText, Key, IV));
    }

    /// <summary>
    /// Decode from base64 and decrypt
    /// </summary>
    public static string Decrypt(string encodedText, byte[] Key, byte[] IV)
    {
        return Decrypt(Convert.FromBase64String(encodedText), Key, IV);
    }
}