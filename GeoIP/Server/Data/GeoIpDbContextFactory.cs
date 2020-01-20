#region HEADER
//   GeoIpDbContextFactory.cs of GeoIP.Server
//   Created by Nikita Neverov at 20.01.2020 13:56
#endregion


using System.IO;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;


namespace GeoIP.Server.Data
{
    public sealed class GeoIpDbContextFactory : IDesignTimeDbContextFactory<GeoIpDbContext>
    {
        public GeoIpDbContext CreateDbContext(string[] args)
        {
            var connection = new ConfigurationBuilder()
                            .SetBasePath(Directory.GetCurrentDirectory())
                            .AddJsonFile(@"Properties/appSettings.json")
                            .Build()
                            .GetConnectionString(@"Default");


            var options = new DbContextOptionsBuilder<GeoIpDbContext>()
                         .UseNpgsql(connection)
                         .Options;

            return new GeoIpDbContext(options);
        }
    }
}