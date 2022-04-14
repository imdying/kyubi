using System.Security.Principal;
namespace V.Components.Commands;

public static class Mount
{
    [Command("Mount", Description = "Mount a remote as a network drive. Additionally, the command also handles the unlocking and locking process of the config file.")]
    public static void Invoke(string key, string remoteName, string volName = "Remote")
    {
        if (string.IsNullOrWhiteSpace(key) || 
            string.IsNullOrWhiteSpace(remoteName) || 
            string.IsNullOrWhiteSpace(volName))
            throw new ArgumentNullException();

        if (Lock.IsLocked())
            Internal.Echo("Unlocking the configuration file.", () => Lock.SetConfigAs(key, (Lock.State)1));
        else
            Internal.Warning("The config file seems to be already unencrypted.");

        // Handling SIGINT Signal
        Console.CancelKeyPress += (s, e) => e.Cancel = true;
        
        Console.WriteLine("Starting rclone.");
        Internal.OpenProcess("rclone.exe",
                             $"mount \"{remoteName}\" * --volname \"{volName}\" --vfs-cache-mode full --network-mode",
                             wait: true,
                             useShellExe: false,
                             asAdmin: IsAdministrator());

        // No need of ManualResetEvent.
        Internal.Warning("Locking the configuration file.");
        Lock.SetConfigAs(key, 0);
    }

    private static bool IsAdministrator() => new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator);
}