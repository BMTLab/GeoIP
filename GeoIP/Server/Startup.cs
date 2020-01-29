#region HEADER
//   Startup.cs of GeoIP.Server
//   Created by Nikita Neverov at 18.01.2020 14:31
#endregion


#define SENSITIVE_DATA_LOGGING

using System.Linq;

using GeoIP.Server.Data;
using GeoIP.Server.Filters;
using GeoIP.Server.Services.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;


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
            services.AddResponseCompression(
                         opts =>
                             opts.MimeTypes = ResponseCompressionDefaults
                                             .MimeTypes.Concat(new[] { "application/octet-stream" }))
                    .AddResponseCaching()
                    .AddMemoryCache();
            #endregion


            #region Commons
            #pragma warning disable 612
            services.AddControllers(o => o.Filters.Add<ValidateRequestAttribute>())
                     #pragma warning restore 612
                    .AddNewtonsoftJson();
            #endregion


            #region Contexts
            services.AddDbContextPool<GeoIpDbContext>(
                options =>
                {
                    options.UseNpgsql(_configuration.GetConnectionString(@"Default"));
                    options.EnableServiceProviderCaching();

                    #if DEBUG || SENSITIVE_DATA_LOGGING
                    options.EnableSensitiveDataLogging();
                    #endif
                });
            #endregion


            services.AddGeoIpProvider();
        }


        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage()
                   .UseBlazorDebugging();
            }
            
            app.UseResponseCompression()
               .UseResponseCaching();

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = ctx =>
                    ctx.Context.Response.Headers.Add("Cache-Control", "public,max-age=1200")
            });
            app.UseClientSideBlazorFiles<Client.Program>();

            app.UseRouting()
               .UseEndpoints(endpoints =>
                {
                    endpoints.MapDefaultControllerRoute();
                    endpoints.MapFallbackToClientSideBlazor<Client.Program>(@"index.html");
                });
        }
        #endregion
    }
}