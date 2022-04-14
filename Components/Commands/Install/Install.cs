using System.IO.Compression;
using System.Net;
using V.Components;

namespace V.Components.Commands;

public static partial class Install
{
    [Command("Install", Description = "Download and install rclone and its required components.")]
    public static void Invoke()
    {
        var IsRcloneInstalled = Rclone.IsInstalled;
        var IsWinFspInstalled = Rclone.IsWinFspInstalled;

        if (IsRcloneInstalled && IsWinFspInstalled)
            Internal.Warning("Rclone and its components are already installed.", true);

        if (!IsRcloneInstalled)
            Internal.Echo("Installing in the current directory...", () => Fetch.Rclone());

        if (!IsWinFspInstalled
            && RequestUserInput(IsRcloneInstalled ? "Missing component. Install WinFsp?" : "WinFsp is required to mount rclone as drives. Install?"))
            Fetch.WinFsp();
    }

    /// <summary>
    /// Extract a zip file.
    /// </summary>
    private static void Extract(string zip, string dst, bool delete = true)
    {
        if (!File.Exists(zip))
            throw new Exception("Cannot find the zip file.");

        ZipFile.ExtractToDirectory(zip,
                                   dst);

        if (delete)
            File.Delete(zip);
    }

    /// <summary>
    /// Generate a randomized directory in %temp% and return its path.
    /// </summary>
    private static string GetTmpPath()
    {
        return Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName())).FullName;
    }

    private static bool RequestUserInput(string question)
    {
        while (true)
        {
            Console.WriteLine(string.Format("{0} (y/n)", question));
            switch (Console.ReadLine()?.ToLower())
            {
                case "y":
                    return true;
                case "n":
                    return false;
            }
        }
    }

    private static byte[] DownloadBytes(string url)
    {
#pragma warning disable SYSLIB0014
        using (var client = new WebClient())
        {
            try
            {
                return client.DownloadData(new Uri(url));
            }
            catch (WebException)
            {
                Internal.Error($"An error occured while downloading data at '{url}'.", true);
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
#pragma warning restore SYSLIB0014
    }

    private static string DownloadString(string url)
    {
        using (var httpClient = new HttpClient())
        using (var requestMessage = new HttpRequestMessage(HttpMethod.Get, url))
        {
            requestMessage.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.62 Safari/537.36");
            var response = httpClient.Send(requestMessage);
            return response.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }
    }

    private static void MoveFiles(string[] files, string dst)
    {
        if (!Directory.Exists(dst))
            throw new Exception("Cannot move files to a path that doesn't exist.");

        for (int i = 0; i < files.Length; i++)
        {
            File.Move(
                files[i],
                Path.Combine(
                    dst,
                    new FileInfo(files[i]).Name
                )
            );
        }
    }

    private static string[] GetFiles(string path)
    {
        return Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);
    }
}