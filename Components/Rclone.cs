using System.ServiceProcess;
namespace V.Components;

public static partial class Rclone
{
    public static class Url
    {
        public static string x86 => "https://downloads.rclone.org/rclone-current-windows-386.zip";

        public static string x64 => "https://downloads.rclone.org/rclone-current-windows-amd64.zip";

        public static string Get() => Environment.Is64BitOperatingSystem ? x64 : x86;

        public static string Configuration => Path.Combine(Directory.GetCurrentDirectory(), "rclone.conf");
    }

    private static bool IsServiceInstalled(string serviceName)
    {
        // get list of Windows services
        ServiceController[] services = ServiceController.GetServices();

        // try to find service name
        foreach (ServiceController service in services)
            if (service.DisplayName == serviceName)
                return true;

        return false;
    }

    public static void SetAsInstalled() => File.Create(".installed");

    public static bool IsInstalled => File.Exists(".installed") == true;

    public static bool IsWinFspInstalled => IsServiceInstalled("WinFsp.Launcher");
}