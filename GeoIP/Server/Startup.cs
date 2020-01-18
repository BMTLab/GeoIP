#region HEADER
//   Startup.cs of GeoIP.Server
//   Created by Nikita Neverov at 18.01.2020 14:31
#endregion


#define SENSITIVE_DATA_LOGGING

using System.Linq;

using GeoIP.Server.Data;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace GeoIP.Server
{
    public sealed class Startup
    {
        #region Fields
        private readonly IConfiguration _configuration;
        #endregion


        #region Constructors
        public Startup(IConfiguration configuration) => _configuration = configuration;
        #endregion


        #region Methods
        public void ConfigureServices(IServiceCollection services)
        {
            #region Caches, Compressions
            services.AddResponseCompression(opts => opts.MimeTypes = ResponseCompressionDefaults
                                                                    .MimeTypes.Concat(
                                                                         new[] { "application/octet-stream" }))
                    .AddResponseCaching();
            #endregion


            #region Commons
            services.AddControllers()
                    .AddNewtonsoftJson();
            #endregion


            #region Contexts
            services.AddEntityFrameworkNpgsql()
                    .AddDbContext<GeoIpDbContext>(
                         options =>
                         {
                             options.UseNpgsql(
                                 _configuration.GetConnectionString(@"Default"));
                             options.EnableServiceProviderCaching();

                             #if DEBUG || SENSITIVE_DATA_LOGGING
                             options.EnableSensitiveDataLogging();
                             #endif
                         });
            #endregion
        }


        public void Configure(IApplicationBuilder app)
        {
            app.UseResponseCompression()
               .UseResponseCaching();

            app.UseDeveloperExceptionPage()
               .UseBlazorDebugging();

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                    ctx.Context.Response.Headers.Add("Cache-Control", "public,max-age=1200")
            });
            app.UseClientSideBlazorFiles<Client.Startup>();

            app.UseRouting()
               .UseEndpoints(endpoints =>
                {
                    endpoints.MapDefaultControllerRoute();
                    endpoints.MapFallbackToClientSideBlazor<Client.Startup>("index.html");
                });
        }
        #endregion
    }
}