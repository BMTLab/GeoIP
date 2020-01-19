#region HEADER
//   Loader.cs of GeoIP.Updater
//   Created by Nikita Neverov at 19.01.2020 17:25
#endregion


using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;


namespace GeoIP.Updater
{
    public static class Loader
    {
        public static AsyncCompletedEventHandler? DownloadResultHandler;
        public static DownloadProgressChangedEventHandler? DownloadProgressHandler;
        
        public static async Task LoadAsync(string sourceUrl, string destinationPath)
        {
            var webClient = new WebClient();

            webClient.DownloadFileCompleted += DownloadResultHandler;
            webClient.DownloadProgressChanged += DownloadProgressHandler;
            
            await webClient.DownloadFileTaskAsync(sourceUrl, destinationPath);
        }
    }
}