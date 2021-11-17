/*using System.IO.Compression;
using System.Net;

using Microsoft.Extensions.Configuration;

using Npgsql;


namespace DataLoader;


/// <summary>
/// Simplified implementation of the database update task.
/// Should be replaced in the future with a parser of command line arguments
/// </summary>
public static class Program
{
    #region Fields
    private static readonly string ApiUrl;
    private static readonly string UpdateFileName;
    private static readonly string UpdateScriptPath;
    private static readonly string IndexScriptPath;
    private static readonly string ConnectionString;
    #endregion


    #region Constructors
    static Program()
    {
        OnNextMessage("Start update ip-db programm");
        OnNextMessage("Read properties");

        var configuration = new ConfigurationBuilder()
                           .SetBasePath(Directory.GetCurrentDirectory())
                           .AddJsonFile(@"Properties/appSettings.json")
                           .Build();

        ApiUrl = configuration.GetSection("GeoLite2")["api"];
        ConnectionString = configuration.GetSection("ConnectionStrings")["Default"];

        UpdateFileName = @"geolitedb.zip";
        UpdateScriptPath = @"Scripts\geoipdb_update.sql";
        IndexScriptPath = @"Scripts\geoipdb_index.sql";
    }
    #endregion


    #region Methods
    public static async Task Main(string[] args)
    {
        try
        {
            OnNextMessage("1/4 Creating directory for updates");
            var updateDirInfo = CreateUpdateDirectory();

            OnNextMessage("2/4 Downloading updates");

            try
            {
                await DownloadCsvAsync(ApiUrl, updateDirInfo.FullName, UpdateFileName);
            }
            catch (WebException exc)
            {
                OnNextMessage("Failed to load updates \r\n" + exc.Message);
            }

            OnNextMessage("3/4 Extracting data from archive");
            var extractDirPath = ExtractCsv(updateDirInfo.FullName, UpdateFileName);

            OnNextMessage("4/4 Database updating");

            try
            {
                await UpdateDatabaseAsync(ConnectionString, extractDirPath, UpdateScriptPath);
            }
            catch (NpgsqlException exc)
            {
                OnNextMessage("Failed to update db \r\n" + exc.Message);
            }
        }
        catch (Exception exc)
        {
            OnNextMessage("Update failed \r\n" + exc.Message);
        }

        OnNextMessage("Completed");
    }


    private static DirectoryInfo CreateUpdateDirectory() =>
        Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}/ArchivesUpdate/");


    private static async Task DownloadCsvAsync(string url, string dirPath, string fileName)
    {
        Console.WriteLine(@$"Start update from: {url}");

        Loader.DownloadResultHandler += async (_, e) =>
        {
            await Task.Delay(500);
            Console.WriteLine(e.Error is null || !e.Cancelled ? "Completed" : "Error");
        };

        Loader.DownloadProgressHandler += async (_, e) =>
        {
            await Task.Delay(5000);
            Console.WriteLine($"{e.ProgressPercentage}% \t {e.BytesReceived}/{e.TotalBytesToReceive}");
        };

        await Loader.LoadAsync(url, dirPath + fileName);
    }


    private static string ExtractCsv(string sourcePath, string fileName)
    {
        var exctractDirRootPath = sourcePath + @"extract";

        if (Directory.Exists(exctractDirRootPath))
            Directory.Delete(exctractDirRootPath, true);

        ZipFile.ExtractToDirectory
        (
            sourcePath + fileName,
            exctractDirRootPath
        );

        return Directory.EnumerateDirectories(exctractDirRootPath).First();
    }


    /// <summary>
    ///     Loading update and index script, that copy and index data to db
    /// </summary>
    /// <param name="connString"></param>
    /// <param name="csvDirPath"></param>
    /// <param name="updateScriptsPath"></param>
    private static async Task UpdateDatabaseAsync(string connString, string csvDirPath, string updateScriptsPath)
    {
        await using var conn = new NpgsqlConnection(connString);
        OnNextMessage("Connection opened");

        /* Load script to copy data to db #1#
        var sqlUpdate = await File.ReadAllTextAsync(updateScriptsPath);
        sqlUpdate = sqlUpdate.Replace(":p", @$"'{csvDirPath}\GeoLite2-City-Locations-en.csv'");
        sqlUpdate = sqlUpdate.Replace(":v", @$"'{csvDirPath}\GeoLite2-City-Blocks-IPv4.csv'");

        /* Load script to index db #1#
        var sqlIndex = await File.ReadAllTextAsync(IndexScriptPath);

        await using var cmdUpdate = new NpgsqlCommand(sqlUpdate, conn);
        await using var cmdIndex = new NpgsqlCommand(sqlIndex, conn);

        await conn.OpenAsync();

        await cmdUpdate.PrepareAsync();
        await cmdIndex.PrepareAsync();

        OnNextMessage
        (
            "Data prepared \r\n" +
            "There is an update. \r\n" +
            "The operation may take a long time. \r\n" +
            "Do not turn off the program"
        );

        await cmdUpdate.ExecuteNonQueryAsync();
        await cmdIndex.ExecuteNonQueryAsync();
    }


    /// <summary>
    ///     Log stub method
    /// </summary>
    /// <param name="status"></param>
    private static void OnNextMessage(string status) =>
        Console.WriteLine(status);
    #endregion _Methods
}*/