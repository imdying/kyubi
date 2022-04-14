using V.Components;
namespace V.Components.Commands;

public static partial class Install
{
    public static partial class Fetch
    {
        public static void Rclone()
        {
            string Tmp = GetTmpPath(),
                   Url = Components.Rclone.Url.Get(),
                   Zip = Path.Combine(Tmp, "rclone-windows.zip");


            Internal.Echo("Downloading...", () =>
            {
                File.WriteAllBytes(Zip,
                                   DownloadBytes(Url));
            });

            Internal.Echo("Extracting...", () =>
            {
                Extract(Zip, Tmp, true);
                MoveFiles(GetFiles(GetRclone(Tmp)), Directory.GetCurrentDirectory());
            });

            Internal.Echo("Finalizing...", () =>
            {
                const string Config = "rclone.conf";

                if (Directory.Exists(Tmp))
                    Directory.Delete(Tmp, true);

                // Creating config
                if (!File.Exists(Config))
                    File.Create(Path.Combine(Directory.GetCurrentDirectory(), Config));

                Internal.Echo("Completed.", () => Components.Rclone.SetAsInstalled());
            });
        }

        /// <summary>
        /// Search for rclone and return its residing directory.
        /// </summary>
        private static string GetRclone(string path)
        {
            return Path.GetDirectoryName(Directory.GetFiles(path, "rclone.exe", SearchOption.AllDirectories)[0])!;
        }
    }
}