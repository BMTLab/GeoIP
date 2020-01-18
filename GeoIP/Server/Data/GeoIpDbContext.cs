#region HEADER
//   GeoIpDbContext.cs of GeoIP.Server
//   Created by Nikita Neverov at 18.01.2020 17:55
#endregion


#define SENSITIVE_DATA_LOGGING

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


namespace GeoIP.Server.Data
{
    public sealed class GeoIpDbContext : DbContext
    {
        #region Fields
        #endregion


        #region Properties.DbSets
        #endregion


        #region Methods
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (optionsBuilder is null || optionsBuilder.IsConfigured)
                return;

            base.OnConfiguring(optionsBuilder);
            var connectionString = Startup.Configuration.GetConnectionString("Default");

            optionsBuilder.UseNpgsql(connectionString);

            #if DEBUG || SENSITIVE_DATA_LOGGING
            optionsBuilder.EnableSensitiveDataLogging();
            #endif
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            if (modelBuilder is null)
                return;

            base.OnModelCreating(modelBuilder);
        }
        #endregion
    }
}