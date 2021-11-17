#define FORCE_CACHING
#define FORCE_SENSITIVE_DATA_LOGGING

using System.IO.Compression;

using Context;

using JetBrains.Annotations;

using Localization.Helpers.Extensions;

using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

using Server.FiltersAttributes;
using Server.Hubs;
using Server.Middlewares.Extensions;
using Server.Models.Constants;
using Server.Services.BackgroundWorkers;
using Server.Services.Providers.Ef;
using Server.Services.Providers.Ef.Abstractions;
using Server.Services.Repositories;
using Server.Services.Repositories.Abstractions;
using Server.Settings;

using Shared.Constants;
using Shared.Services.Configuration;
using Shared.Services.Configuration.Extensions;
using Shared.Services.Crypto;
using Shared.Services.Crypto.Decryptor;
using Shared.Services.SecretsBridge;
using Shared.Utils.TypeExtensions.JsonSerializerExtensions;
using Shared.ViewModels;
using Shared.ViewModels.Validators;
using Shared.ViewModels.Validators.Extensions;


namespace Server;


[UsedImplicitly]
public sealed class Startup
{
    #region Ctors
    public Startup(IConfiguration configuration, IWebHostEnvironment environment)
    {
        _environment = environment;
        _configuration = configuration;
    }
    #endregion


    #region Properties
    internal static IServiceProvider? Provider { get; private set; }
    #endregion _Properties


    #region Fields
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;
    #endregion


    public void ConfigureServices(IServiceCollection services)
    {
        #region Caches
        services.AddResponseCaching(options => options.UseCaseSensitivePaths = false);
        services.AddResponseCompression()
                .Configure<BrotliCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest)
                .Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Optimal);

        services.AddMemoryCache();
        #endregion _Caches


        #region Localization
        services.AddDynamicLocalization();
        #endregion


        #region Configurations&Options
        var cryptoAlg = new CryptoFactory().CreateAes(CredentialHolder.Password, CredentialHolder.Salt);
        var decryptor = new SecretDecryptorFactory().Create(cryptoAlg);
        services.AddSingleton(cryptoAlg);
        services.AddSingleton(decryptor);

        services.AddSectionOptions<AppSecrets>(_configuration);
        services.AddSingleton<ISecretValidator<IAppSecretsStructure>, SecretValidator<IAppSecretsStructure>>();
        services.AddSingleton<IAppSecretsResolved, AppSecretBridge>();

        // Add the other interfaces implemented by AppSecretBridge to allow for resolution by those interfaces (interface segregation)
        //services.AddSingleton<IJwtOptions>(provider => provider.GetRequiredService<IAppSecretsResolved>());
        #endregion


        #region Context
        services.AddDbContextPool<GeoIpDbContext>
        (
            options =>
            {
                options.UseNpgsql
                (
                    GeoIpDbContextFactory.GetConnectionString(_environment.EnvironmentName),
                    npgslOptions =>
                    {
                        npgslOptions.EnableRetryOnFailure(16, TimeSpan.FromSeconds(32), null);
                        npgslOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                    }
                );
            }
        );
        #endregion
        

        #region Cors
        services.AddCors();
        #endregion _Cors


        #region Commons
        services.AddRouting();
        services.AddControllers
                 (
                     options =>
                     {
                         options.CacheProfiles.Add
                         (
                             CacheProfileNames.Long,
                             new CacheProfile
                             {
                                 Duration = 60,
                                 Location = ResponseCacheLocation.Any
                             }
                         );

                         options.OutputFormatters.RemoveType<StringOutputFormatter>();
                         options.ReturnHttpNotAcceptable = true;

                         options.Filters.Add<ExceptionCatchFilterAttribute>();
                         options.Filters.Add<AppVersionHeaderFilterAttribute>();
                     }
                 )
                .ConfigureApiBehaviorOptions
                 (
                     options =>
                     {
                         options.SuppressModelStateInvalidFilter = true;
                         options.SuppressMapClientErrors = true;
                     }
                 )
                .AddJsonOptions
                 (
                     options =>
                     {
                         options.JsonSerializerOptions.SetOptions();
                     }
                 );

        services.AddSignalR
                 (
                     options =>
                     {
                         #if !RELEASE
                         options.EnableDetailedErrors = true;
                         #endif

                         options.MaximumParallelInvocationsPerClient = 2;
                         options.KeepAliveInterval = TimeSpan.FromSeconds(10);
                         options.ClientTimeoutInterval = TimeSpan.FromHours(1);
                     }
                 )
                .AddJsonProtocol
                 (
                     options => { options.PayloadSerializerOptions = GeoIpJsonSerializerOptions.Instance; }
                 );


        #region Swagger
        services.AddSwaggerGen
        (
            c =>
            {
                var name = GeoIpEnvironment.ServerName;
                var version = GeoIpEnvironment.ServerVersion;
                var description = GeoIpEnvironment.ServerDescription ?? @$"{GeoIpEnvironment.ServerName} API";

                c.SwaggerDoc
                (
                    version,
                    new OpenApiInfo
                    {
                        Title = name,
                        Version = version,
                        Description = description
                    }
                );
            }
        );
        #endregion


        #region Validators
        services.AddLocalizedValidators
        (
            typeof(IpAddressModelValidator).Namespace!,
            typeof(IpAddressModel).Namespace!
        );
        #endregion _Validators


        #region Providers
        services.AddScoped<IGeoIpProvider, GeoIpProvider>();
        #endregion


        #region Repositories
        services.AddScoped<IGeoIpRepository, GeoIpRepository>();
        #endregion _Repositories
        
        services.AddHostedService<UpdateDbBackgroundService>();

        Provider = services.BuildServiceProvider();
    }


    public static void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime lifetime)
    {
        Console.WriteLine($"\nEnvironment: {env.EnvironmentName}");

        app.UseForwardedHeaders(new ForwardedHeadersOptions { ForwardedHeaders = ForwardedHeaders.All });
        app.UseStatusCodePages();
        
        #if DEBUG
        //app.UseBrowserLink();
        //app.UseWebAssemblyDebugging();
        #endif

        app.UseCors
        (
            options =>
            {
                options.AllowAnyOrigin();
                options.AllowAnyMethod();
                options.AllowAnyHeader();
            }
        );

        #if !DEBUG || FORCE_CACHING
        app.UseResponseCaching();
        app.UseResponseCompression();
        #endif

        //app.UseBlazorFrameworkFiles();
        //app.UseStaticFiles(GeoIpEnvironment.StaticFileOptions);

        app.UseSwagger();
        app.UseSwaggerUI
        (
            c => c.SwaggerEndpoint
            (
                $"/swagger/{GeoIpEnvironment.ServerVersion}/swagger.json",
                $"{GeoIpEnvironment.ServerName} {GeoIpEnvironment.ServerVersion}"
            )
        );

        app.UseRouting();

        app.UseAuthentication()
           .UseAuthorization();

        app.UseServerLocalization();

        app.UseEndpoints
        (
            endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ClientHub>(Urls.Hub.Client.Base);

                //endpoints.MapFallbackToFile(@"index.min.html");
            }
        );
        
        lifetime.ApplicationStarted.Register(() => Console.WriteLine("\nApplication started"));
        lifetime.ApplicationStopping.Register(() => Console.WriteLine("\nApplication stopping"));
        lifetime.ApplicationStopped.Register(() => Console.WriteLine("\nApplication stopped"));
    }
    #endregion
}