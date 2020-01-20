#region HEADER
//   GeolocationByIpController.cs of GeoIP.Server
//   Created by Nikita Neverov at 18.01.2020 14:31
#endregion


using GeoIP.Server.Filters;
using GeoIP.Server.Services.DataProviders;

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
            var y = _provider.GetAllInfoByIp(ip);

            return new JsonResult(new { Y = y?.Location.CityName });
        }
        #endregion _Methods.HTTP
    }
}