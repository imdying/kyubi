namespace V.Components.Commands;

public class Ls
{
    [Command("Ls", Description = "Display the lock-status of the configuration file.")]
    public static void Invoke() => Console.WriteLine("The content is: {0}", Lock.IsLocked() ? "Encrypted" : "Not encrypted");
}