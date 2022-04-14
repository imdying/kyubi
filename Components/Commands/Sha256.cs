namespace V.Components.Commands;

public static class Sha256
{
    [Command("Sha256", Description = "Secure Hash Algorithm 256-bit")]
    public static void Invoke(string key) => Console.WriteLine(Crytography.Sha256.GetHash(key));
}