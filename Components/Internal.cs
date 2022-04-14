using System.Diagnostics;
namespace V.Components;

public static class Internal
{
    public static void ExitIf(bool condition, int code = 0)
    {
        if (condition)
            Environment.Exit(code);
        else
            return;
    }

    public static void Echo(string str, Action action)
    {
        WriteLine(str);
        action?.Invoke();
    }

    public static void Error(string str, bool exit = false)
    {
        WriteLine(str,
                  ConsoleColor.Red,
                  exit);
    }

    public static void Warning(string str, bool exit = false)
    {
        WriteLine(str,
                  ConsoleColor.Yellow,
                  exit);
    }

    public static void WriteLine(string str, ConsoleColor? color = null, bool exit = false)
    {
        Console.ForegroundColor = color ?? Console.ForegroundColor;
        Console.WriteLine(str);
        Console.ResetColor();
        ExitIf(exit);
    }

    internal static void OpenProcess(string file, string? args, bool wait = false, bool useShellExe = false, bool asAdmin = false, bool isHidden = false, string? workdir = null)
    {
        using (var _process = new Process())
        {
            var StartInfo = new ProcessStartInfo()
            {
                Verb = asAdmin ? "runas" : null,
                UseShellExecute = useShellExe,
                FileName = file,
                WindowStyle = isHidden ? ProcessWindowStyle.Hidden : ProcessWindowStyle.Normal,
                Arguments = args
            };

            if (!string.IsNullOrWhiteSpace(workdir))
                StartInfo.WorkingDirectory = workdir;

            _process.StartInfo = StartInfo;
            _process.Start();

            if (wait)
                _process.WaitForExit();
        }
    }
}
