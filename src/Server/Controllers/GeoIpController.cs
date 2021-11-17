using System.Diagnostics.CodeAnalysis;

using Localization;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Server.Controllers.Abstractions;
using Server.Models.Constants;
using Server.Services.Repositories.Abstractions;

using Shared.Models;
using Shared.ViewModels;
using Shared.ViewModels.Results;


namespace Server.Controllers;


[AllowAnonymous]
[ResponseCache(CacheProfileName = CacheProfileNames.Long)]
[SuppressMessage("ReSharper", "UnusedParameter.Local")]
public sealed class GeoIpController : CustomControllerBase<GeoIpController>
{
    private readonly IGeoIpRepository _geoIpRepository;


    #region Ctors
    public GeoIpController
    (
        IGeoIpRepository geoIpRepository,
        ILocalization localization,
        ILogger<GeoIpController>? logger = null
    ) : base(localization, logger)
    {
        _geoIpRepository = geoIpRepository;
    }
    #endregion


    #region Mehods.HTTP
    [HttpGet]
    [ProducesResponseType(typeof(Block), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResult), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get([FromQuery] IpAddressModel model)
    {
        if (IsValidationFailed(model, out var badResult))
            return badResult;
        
        var result = await _geoIpRepository.GetByIpAsync(model.Ip!);

        return result.Match
        (
            block => (IActionResult) Ok(block),
            error => NotFound(new ErrorResult(L.ErrorNotFoundLocation))
        );
    }
    #endregion _Methods.HTTP
}