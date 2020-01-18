#region HEADER
//   Startup.cs of GeoIP.Server
//   Created by Nikita Neverov at 18.01.2020 14:31
#endregion


using System.Linq;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;


namespace GeoIP.Server
{
    public sealed class Startup
    {
        #region Constructors
        public Startup(IConfiguration configuration) => Configuration = configuration;
        #endregion


        #region Properties
        public static IConfiguration Configuration { get; private set; }
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
        }


        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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