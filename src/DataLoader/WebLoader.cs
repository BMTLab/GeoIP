using System.Net;
#pragma warning disable CS0618
#pragma warning disable SYSLIB0014


namespace DataLoader;

public static class WebLoader
{
    /// <summary>
    /// Downloads a file using <see cref="WebClient"/>
    /// </summary>
    /// <remarks>
    /// <see cref="WebClient"/> is obsolete. For demo only
    /// </remarks>
    /// <param name="sourceUrl"></param>
    /// <param name="destinationPath"></param>
    /// <param name="cancellationToken"></param>
    /// <returns></returns>
    public static Task LoadFileTask(Uri sourceUrl, string destinationPath, CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        using var webClient = new WebClient();
        
        return webClient.DownloadFileTaskAsync(sourceUrl, destinationPath);
        
    }
}