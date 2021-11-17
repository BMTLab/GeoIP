using Localization;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Server.Controllers.Abstractions;
using Server.Models.Constants;
using Server.Settings;

using Shared.Models.Server;


namespace Server.Controllers;

[AllowAnonymous]
[ResponseCache(CacheProfileName = CacheProfileNames.No)]
public sealed class ServerController : CustomControllerBase<ServerController>
{
    #region Fields
    private readonly IWebHostEnvironment _environment;
    #endregion


    #region Ctors
    public ServerController
    (
        IWebHostEnvironment environment,
        ILocalization localization,
        ILogger<ServerController>? logger = null
    ) : base(localization, logger)
    {
        _environment = environment;
    }
    #endregion


    #region Mehods.HTTP
    [HttpHead]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    public IActionResult Test() =>
        Ok(true);


    [HttpGet]
    [ProducesResponseType(typeof(ServerInfo), StatusCodes.Status200OK)]
    public IActionResult GetInfo() =>
        Ok
        (
            new ServerInfo
            {
                Name = GeoIpEnvironment.ServerName,
                Version = GeoIpEnvironment.ServerVersion,
                Environment = _environment.EnvironmentName,
                Language = L.Lang,
                HelloMessage = L.Hello
            }
        );
    #endregion _Methods.HTTP
}