#region HEADER
//   Loader.cs of GeoIP.Updater
//   Created by Nikita Neverov at 20.01.2020 8:11
#endregion


using System.ComponentModel;
using System.Net;
using System.Threading.Tasks;

using Fody;


namespace GeoIP.Updater
{
    [ConfigureAwait(false)]
    public static class Loader
    {
        #region Fields.Handlers
        public static AsyncCompletedEventHandler? DownloadResultHandler;
        public static DownloadProgressChangedEventHandler? DownloadProgressHandler;
        #endregion
        
        
        #region Methods
        public static async Task LoadAsync(string sourceUrl, string destinationPath)
        {
            using var webClient = new WebClient();

            webClient.DownloadFileCompleted += DownloadResultHandler;
            webClient.DownloadProgressChanged += DownloadProgressHandler;

            await webClient.DownloadFileTaskAsync(sourceUrl, destinationPath);
        }
        #endregion
    }
}