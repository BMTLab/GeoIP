#region HEADER
//   GeolocationByIpController.cs of GeoIP.Server
//   Created by Nikita Neverov at 18.01.2020 14:31
#endregion


using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Fody;

using GeoIP.Server.Services.DataProviders;
using GeoIP.Shared.Models;
using GeoIP.Shared.ViewModels;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace GeoIP.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ConfigureAwait(false)]
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
        /// <summary>
        /// HTTP GET: GeolocationByIp/Get
        /// </summary>
        /// <param name="ip">Requested ip</param>
        /// <see href="http://localhost/api/geolocationbyip"/>
        /// <returns>JSON</returns>
        [HttpGet("{ip}")]
        [ActionName("Get")]
        //[ValidateRequest]
        public async Task<IActionResult> GetAsync(string ip)
        {
            Block? ipInfo = null;
            
            const string ipPattern = "^((25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)$";

            if (string.IsNullOrWhiteSpace(ip) || !Regex.IsMatch(ip, ipPattern))
            {
                return BadRequest(new RequestResult { Successful = false, Error = @"Invalid Ip" });
            }

            try
            {
                ipInfo = await _provider.GetAllInfoByIpAsync(ip);
            }
            catch (Exception exc)
            {
                _logger?.LogError(exc.Message);
                
                return BadRequest(new RequestResult { Successful = false, Error = @"Server error" });
            }
            
            if (ipInfo is null)
            {
                return BadRequest(new RequestResult { Successful = false, Error = @"Ip not found" });
            }

            return Ok(ipInfo);
        }
        #endregion _Methods.HTTP
    }
}