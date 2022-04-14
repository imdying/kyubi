namespace V.Components.Crytography;

public static class BootlegHash
{
    public static byte[] GetHash(string key)
    {
        return Sha256.GetByteHash(key).Chunk(16).First();
    }
}