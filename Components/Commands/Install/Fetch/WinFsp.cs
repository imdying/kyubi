using System.Diagnostics;
using Newtonsoft.Json;
using V.Components;

namespace V.Components.Commands;

public static partial class Install
{
    public static partial class Fetch
    {
        public static void WinFsp()
        {
            var data = DownloadString("https://api.github.com/repos/winfsp/winfsp/releases/latest");
            dynamic stuff = JsonConvert.DeserializeObject(data)!;

            if (stuff == null)
                throw new Exception("Data is null.");

            var asset = stuff.assets[0];
            string url = asset.browser_download_url;
            string file = Path.Combine(GetTmpPath(), (string)asset.name);

            Internal.Echo("Downloading...", () => File.WriteAllBytes(file, DownloadBytes(url)));
            Internal.Echo("Executing setup...", () =>
            {
                Internal.OpenProcess("cmd.exe", $"/c call \"{file}\"", true);
                File.Delete(file);
            });
        }
    }
}