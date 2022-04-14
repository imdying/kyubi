
using System.Security.Cryptography;
using Crypt = System.Security.Cryptography;

namespace V.Components.Crytography;

public static class Aes
{
    public static byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
    {
        // Check arguments.
        if (plainText == null || plainText.Length <= 0)
            throw new ArgumentNullException("plainText");
        if (Key == null || Key.Length <= 0)
            throw new ArgumentNullException("Key");
        if (IV == null || IV.Length <= 0)
            throw new ArgumentNullException("IV");

        // Create an Aes object
        // with the specified key and IV.
        using (var aesAlg = Crypt.Aes.Create())
        {
            // Create an encryptor to perform the stream transform.
            ICryptoTransform encryptor = aesAlg.CreateEncryptor(Key, IV);

            // Create the streams used for encryption.
            using (MemoryStream msEncrypt = new MemoryStream())
            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            {
                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                {
                    //Write all data to the stream.
                    swEncrypt.Write(plainText);
                }
                return msEncrypt.ToArray();
            }
        }
    }

    public static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
    {
        if (cipherText == null || cipherText.Length <= 0 ||
            key == null || key.Length <= 0 ||
            iv == null || iv.Length <= 0)
            throw new ArgumentNullException();

        // Create an Aes object
        // with the specified key and IV.
        using (var aesAlg = Crypt.Aes.Create())
        {
            // Create a decryptor to perform the stream transform.
            ICryptoTransform decryptor = aesAlg.CreateDecryptor(key, iv);

            // Create the streams used for decryption.
            using (MemoryStream msDecrypt = new MemoryStream(cipherText))
            using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
            using (StreamReader srDecrypt = new StreamReader(csDecrypt))
            {
                // Read the decrypted bytes from the decrypting stream
                // and place them in a string.
                return srDecrypt.ReadToEnd();
            }
        }
    }
}