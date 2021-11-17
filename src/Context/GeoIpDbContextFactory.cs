using System.Diagnostics.CodeAnalysis;

using JetBrains.Annotations;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

using Shared.Services.Configuration;
using Shared.Services.Configuration.Extensions;
using Shared.Services.Crypto;
using Shared.Services.Crypto.Decryptor;
using Shared.Services.Crypto.Decryptor.Extensions;
using Shared.Services.SecretsBridge;
using Shared.Services.SecretsBridge.SecretsModels;
using Shared.Utils.TypeExtensions;


namespace Context;


/// <summary>
///     Serves to work EF Core tools,
///     eliminating the need to have an empty public constructor in the DbContext class
/// </summary>
/// <remarks>
///     The presence of an empty constructor does not allow the use of DbContextPool
/// </remarks>
[UsedImplicitly]
public class GeoIpDbContextFactory : IDesignTimeDbContextFactory<GeoIpDbContext>
{
    public GeoIpDbContext CreateDbContext(string[]? args)
    {
        var env = GetEnvironment();
        var options = new DbContextOptionsBuilder<GeoIpDbContext>()
                     .UseNpgsql
                      (
                          GetConnectionString(env, args),
                          npgslOptions =>
                          {
                              npgslOptions.EnableRetryOnFailure(16, TimeSpan.FromSeconds(32), null);
                              npgslOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);

                              npgslOptions.MigrationsAssembly(env == "Development" ? "Server-Dev" : "Server");
                          }

                      )
                     .UseSnakeCaseNamingConvention()
                     .Options;

        return new GeoIpDbContext(options);
    }


    [SuppressMessage("Globalization", "CA1303", MessageId = "Do not pass literals as localized parameters")]
    [SuppressMessage("ReSharper", "ConvertToConstant.Local")]
    protected virtual string GetEnvironment()
    {
        var defaultEnv = "Development";

        Console.Write($"Enter stage (Default \'{defaultEnv}\'): ");
        var env = Console.ReadLine();
            
        return env.IsValid() ? env : defaultEnv;
    }
        
    
    public static string GetConnectionString(string env, string[]? args = null)
    {
        var cryptoAlg = new CryptoFactory().CreateAes(CredentialHolder.Password, CredentialHolder.Salt);
        var decryptor = new SecretDecryptorFactory().Create(cryptoAlg);
            
        var connectionString = ConfigurationFactory.CreateConfiguration(args, env)
                                      .GetFrom<AppSecrets>()
                                      .ConnectionOptions[nameof(IConnectionOptions.GeoIpDb)]
                                      .ConnectionString
                                      .DecryptValue(decryptor);

        #if !RELEASE
        Console.WriteLine($"Connection string: {connectionString}");
        #endif

        return connectionString;
    }
}