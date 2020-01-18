#region HEADER
//   GeolocationByIpController.cs of GeoIP.Server
//   Created by Nikita Neverov at 18.01.2020 14:31
#endregion


using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace GeoIP.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public sealed class GeolocationByIpController : ControllerBase
    {
        #region Fields
        private readonly ILogger<GeolocationByIpController> _logger;
        #endregion


        #region Constructors
        public GeolocationByIpController
        (
            ILogger<GeolocationByIpController> logger
        )
        {
            _logger = logger;
        }
        #endregion


        #region Methods.HTTP
        #endregion _Methods.HTTP
    }
}