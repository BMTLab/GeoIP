using System.IO.Compression;

using Context;

using DataLoader.Options;

using JetBrains.Annotations;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Shared.Utils.TypeExtensions;


namespace DataLoader;


/// <summary>
/// Naive implementation of the database update process
/// </summary>
public class GeoLiteProvider
{
    private readonly GeoLiteOptions _options;
    private readonly ILogger<GeoLiteProvider>? _logger;

    
    #region Ctors
    public GeoLiteProvider
    (
        GeoLiteOptions options,
        ILogger<GeoLiteProvider>? logger = null
    )
    {
        _options = options;
        _logger = logger;
    }
    #endregion


    #region Methods
    /// <summary>
    /// Runs the algorithm for updating the database.
    /// Deletes all data from tables, loads a file with new data and copies it back to the database
    /// </summary>
    [PublicAPI]
    public async Task UpdateAsync(GeoIpDbContext dbContext, CancellationToken cancellationToken)
    {
        _logger?.LogEntryMethod();

        try
        {
            cancellationToken.ThrowIfCancellationRequested();
            
            var updateDirInfo = CreateUpdateDirectory(Path.Combine(Path.GetTempPath(), _options.ArchiveDirectoryName));
            await DownloadCsvTask(new Uri(_options.Api), updateDirInfo.FullName, _options.ArchiveName, cancellationToken);
            var extractDirPath = ExtractCsv(updateDirInfo.FullName, _options.ArchiveName);
            var sqlUpdateScript = GetUpdateSqlScript();
            var sqlIndexScript = GetIndexSqlScript();
            var affectedRowsCount = await UpdateDatabaseTask(dbContext, extractDirPath, sqlUpdateScript, cancellationToken);
            
            if (affectedRowsCount > 0)
                await IndexDatabaseTask(dbContext, sqlIndexScript, cancellationToken);
        }
        catch (Exception exc) when (exc.GetType() != typeof(OperationCanceledException))
        {
            _logger?.LogError(exc, "An error occurred while updating the database of IP addresses");

            throw;
        }

    }
    
    private static DirectoryInfo CreateUpdateDirectory(string pathToDownload) =>
        Directory.CreateDirectory(pathToDownload);


    private static Task DownloadCsvTask
    (
        Uri url,
        string dirPath,
        string fileName,
        CancellationToken cancellationToken = default
    ) =>
        WebLoader.LoadFileTask(url, Path.Combine(dirPath, fileName), cancellationToken);


    private static string ExtractCsv(string sourcePath, string fileName)
    {
        var exctractDirRootPath = Path.Combine(sourcePath, @"extracted");

        if (Directory.Exists(exctractDirRootPath))
            Directory.Delete(exctractDirRootPath, true);

        ZipFile.ExtractToDirectory
        (
            Path.Combine(sourcePath, fileName),
            exctractDirRootPath
        );

        return Directory.EnumerateDirectories(exctractDirRootPath).First();
    }


    private static string GetUpdateSqlScript() =>
        Context.Utils.ScriptResourceReader.ReadResource<GeoLiteProvider>(@"DataLoader.Scripts.geoipdb_update.sql");
    
    private static string GetIndexSqlScript() =>
        Context.Utils.ScriptResourceReader.ReadResource<GeoLiteProvider>(@"DataLoader.Scripts.geoipdb_index.sql");


    /// <summary>
    ///     Loading update and index script, that copy and index data to db
    /// </summary>
    /// <param name="dbContext"></param>
    /// <param name="csvDirPath"></param>
    /// <param name="sqlUpdateScript"></param>
    /// <param name="cancellationToken"></param>
    private static Task<int> UpdateDatabaseTask
    (
        DbContext dbContext,
        string csvDirPath,
        string sqlUpdateScript,
        CancellationToken cancellationToken = default
    )
    {
        cancellationToken.ThrowIfCancellationRequested();
        
        sqlUpdateScript =
            sqlUpdateScript
               .Replace(":p", @$"'{Path.Combine(csvDirPath, "GeoLite2-City-Locations-en.csv")}'", StringComparison.Ordinal)
               .Replace(":v", @$"'{Path.Combine(csvDirPath, "GeoLite2-City-Blocks-IPv4.csv")}'", StringComparison.Ordinal);

        dbContext.Database.SetCommandTimeout(TimeSpan.FromMinutes(5));
        return dbContext.Database.ExecuteSqlRawAsync(sqlUpdateScript, cancellationToken);
    }
    
    
    private static Task<int> IndexDatabaseTask
    (
        GeoIpDbContext dbContext,
        string sqlIndexScript,
        CancellationToken cancellationToken = default
    )
    {
        dbContext.Database.SetCommandTimeout(TimeSpan.FromMinutes(5));
        return dbContext.Database.ExecuteSqlRawAsync(sqlIndexScript, cancellationToken);
    }
    #endregion _Methods
}