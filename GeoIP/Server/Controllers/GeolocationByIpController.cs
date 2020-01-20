#region HEADER
//   GeolocationByIpController.cs of GeoIP.Server
//   Created by Nikita Neverov at 18.01.2020 14:31
#endregion


using System;
using System.Net;

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
        private readonly ILogger<GeolocationByIpController> _logger;
        #endregion
        
        
        #region Constructors
        public GeolocationByIpController
        (
            IGeoIpProvider provider,
            ILogger<GeolocationByIpController> logger
        )
        {
            _provider = provider;
            _logger = logger;
        }
        #endregion
        
        
        #region Methods.HTTP
        [HttpGet("{ip}")]
        [ValidateRequest]
        public IActionResult Get(string ip)
        {
            Block ipInfo = null;
            
            if (!IPAddress.TryParse(ip, out _))
            {
                ModelState.AddModelError("Not recognized", "IP not recognized");

                goto Falied;
            }

            try
            {
                ipInfo = _provider.GetAllInfoByIp(ip);
            }
            catch (Exception exc)
            {
                _logger.LogError(exc.Message);
                
                goto Falied;
            }

            if (ipInfo is null)
            {
                ModelState.AddModelError("Not found", "IP not found or not correct");
            }
            
            return new JsonResult(ipInfo);
            
            Falied:
            return BadRequest();
        }
        #endregion _Methods.HTTP
    }
}