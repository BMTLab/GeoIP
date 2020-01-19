#region HEADER
//   Program.cs of GeoIP.Updater
//   Created by Nikita Neverov at 19.01.2020 16:10
#endregion


using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using Microsoft.Extensions.Configuration;

using Npgsql;


namespace GeoIP.Updater
{
    public static class Program
    {
        #region Fields
        private static readonly string ApiUrl;
        private static readonly string UpdateFileName;
        private static readonly string UpdateScriptPath;
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
                    await DownloadCsvAsync(ApiUrl, updateDirInfo.FullName, UpdateFileName).ConfigureAwait(false);
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
                    await UpdateDatabaseAsync(ConnectionString, extractDirPath, UpdateScriptPath).ConfigureAwait(false);
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


        private static DirectoryInfo CreateUpdateDirectory() 
            => Directory.CreateDirectory($"{Directory.GetCurrentDirectory()}/ArchivesUpdate/");


        private static async Task DownloadCsvAsync(string url, string dirPath, string fileName)
        {
            Console.WriteLine(@$"Start update from: {url}");
            
            Loader.DownloadResultHandler += async (_, e) =>
            {
                await Task.Delay(500).ConfigureAwait(false);
                Console.WriteLine(e.Error is null || !e.Cancelled ? "Completed" : "Error");
            };
            
            Loader.DownloadProgressHandler += async (_, e) =>
            {
                await Task.Delay(5000).ConfigureAwait(false);
                Console.WriteLine($"{e.ProgressPercentage}% \t {e.BytesReceived}/{e.TotalBytesToReceive}");
            };
            
            await Loader.LoadAsync(url, dirPath + fileName).ConfigureAwait(false);
        }
        
        private static string ExtractCsv(string sourcePath, string fileName)
        {
            var exctractDirRootPath = sourcePath + @"extract";
            
            if (Directory.Exists(exctractDirRootPath))
                Directory.Delete(exctractDirRootPath, true);
            
            ZipFile.ExtractToDirectory(sourcePath + fileName,
                                       exctractDirRootPath);

            return Directory.EnumerateDirectories(exctractDirRootPath).First();
        }
        
        private static async Task UpdateDatabaseAsync(string connString, string csvDirPath, string updateScriptsPath)
        {
            var sql = await File.ReadAllTextAsync(updateScriptsPath);
            sql = sql.Replace(":p", @$"'{csvDirPath}\GeoLite2-City-Locations-ru.csv'");
            sql = sql.Replace(":v", @$"'{csvDirPath}\GeoLite2-City-Blocks-IPv4.csv'");
            
            await using var conn = new NpgsqlConnection(connString);
            await using var cmd = new NpgsqlCommand(sql, conn);
            
            await conn.OpenAsync().ConfigureAwait(false);
            OnNextMessage("Connection opened");
            
            await cmd.PrepareAsync().ConfigureAwait(false);
            OnNextMessage("Data prepared \r\n" +
                          "There is an update. \r\n" +
                          "The operation may take a long time. \r\n" +
                          "Do not turn off the program");
            
            await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void OnNextMessage(string status) 
            => Console.WriteLine(status);
        #endregion _Methods
    }
}