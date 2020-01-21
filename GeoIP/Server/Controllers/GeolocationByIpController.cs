#region HEADER
//   GeolocationByIpController.cs of GeoIP.Server
//   Created by Nikita Neverov at 18.01.2020 14:31
#endregion


using System;
using System.Net;
using System.Threading.Tasks;

using GeoIP.Server.Filters;
using GeoIP.Server.Services.DataProviders;
using GeoIP.Shared.Models;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace GeoIP.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public sealed class GeolocationByIpController : ControllerBase
    {
        #region Fields
        private readonly IGeoIpProvider _provider;
        private readonly ILogger<GeolocationByIpController>? _logger;
        #endregion
        
        
        #region Constructors
        public GeolocationByIpController
        (
            IGeoIpProvider provider,
            ILogger<GeolocationByIpController>? logger = null
        )
        {
            _provider = provider;
            _logger = logger;
        }
        #endregion
        
        
        #region Methods.HTTP
        [HttpGet("{ip}")]
        [ActionName("Get")]
        [ValidateRequest]
        public async Task<IActionResult> GetAsync(string ip)
        {
            Block? ipInfo = null;

            if (!IPAddress.TryParse(ip, out _))
            {
                ModelState.AddModelError("Error", "IP incorrect");

                goto Failed;
            }

            try
            {
                ipInfo = await _provider.GetAllInfoByIpAsync(ip).ConfigureAwait(false);
            }
            catch (Exception exc)
            {
                _logger?.LogError(exc.Message);

                ModelState.AddModelError("Error", "Server error");
                
                goto Failed;
            }
            
            if (ipInfo is null)
            {
                ModelState.AddModelError("Error", "IP not found");
                
                goto Failed;
            }

            return new JsonResult(ipInfo);
            
            Failed:
            return new BadRequestResult();
        }
        #endregion _Methods.HTTP
    }
}