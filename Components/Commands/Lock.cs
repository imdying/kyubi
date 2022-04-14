namespace V.Components.Commands;

public static class Lock
{
    [Command("Lock", Description = "Un/Lock the configuration file's content.")]
    public static void Invoke(string key)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentNullException();

        if (IsLocked())
            Internal.Echo("Unlocking...", () => SetConfigAs(key, State.Unlocked));
        else
            Internal.Echo("Locking...", () => SetConfigAs(key, State.Locked));
    }

    internal enum State
    {
        Locked,
        Unlocked
    }

    private const string FileName = ".last_state";

    internal static bool IsLocked() => File.Exists(FileName) && File.ReadAllText(FileName) == nameof(State.Locked);

    private static void SetLockStatus(State current) => File.WriteAllText(FileName, current.ToString());

    private static void WriteFile(string key)
    {
        if (!File.Exists(Rclone.Url.Configuration))
            throw new FileNotFoundException();

        var Read = File.ReadAllBytes(
            Rclone.Url.Configuration
        );

        if (Read.Length == 0)
            Internal.Error("Empty configuration file.", true);

        var Manipulated = Components.Crytography.Xor.Encrypt(
            Read,
            key
        );

        File.WriteAllBytes(
            Rclone.Url.Configuration,
            Manipulated
        );
    }

    internal static void SetConfigAs(string key, State current)
    {
        WriteFile(key);
        SetLockStatus(current);
    }
}