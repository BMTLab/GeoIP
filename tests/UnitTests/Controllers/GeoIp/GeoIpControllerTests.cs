using Localization;

using Microsoft.AspNetCore.Mvc;

using Moq;

using Server.Controllers;
using Server.Services.Repositories;
using Server.Services.Repositories.StateModels;

using Shared.Models;
using Shared.Utils.TypeExtensions.JsonSerializerExtensions;
using Shared.ViewModels;
using Shared.ViewModels.Results;

using Xunit;
using Xunit.Abstractions;


namespace GeoIp.Tests.Controllers.GeoIp;


public sealed partial class GeoIpControllerTests
{
    #region Fields
    private static readonly List<string> StoredNetworks = new() { "33.33.33.33" };

    private readonly Mock<GeoIpRepository> _geoIpRepositoryMock;
    private readonly ILocalization _localization;
    private readonly ITestOutputHelper _output;
    #endregion
    
    
    #region Ctors
    public GeoIpControllerTests(ITestOutputHelper output)
    {
        // Init mocks
        _geoIpRepositoryMock = new Mock<GeoIpRepository>();
        _localization = new LocalizationEn();
        
        // Setup mocks
        SetupServices();

        // Init log output
        _output = output;
    }
    #endregion
    

    #region Methods.UnitTests
    [Fact]
    public async void Get_ReturnNotfoundIfNotExist()
    {
        //// Arange
        var controller = new GeoIpController
        (
            _geoIpRepositoryMock.Object,
            _localization
        );
        

        //// Act
        var result = await controller.Get(new IpAddressModel { Ip = "22.22.22.22" });


        //// Assert
        Assert.NotNull(result);
        var actionResult = Assert.IsAssignableFrom<IActionResult>(result);
        var notFoundObjectResult = Assert.IsType<NotFoundObjectResult>(actionResult);
        Assert.NotNull(notFoundObjectResult);

        _output.WriteLine(notFoundObjectResult.Value.Serialize());

        var errorResult = Assert.IsType<ErrorResult>(notFoundObjectResult.Value);
        
        Assert.NotEmpty(errorResult.Errors);
    }
    
    
    [Theory]
    [ClassData(typeof(CorrectIpModelModelTestData))]
    public async void Get_ReturnBlockIfCorrect(IpAddressModel testedModel)
    {
        //// Arange
        var controller = new GeoIpController
        (
            _geoIpRepositoryMock.Object,
            _localization
        );
        

        //// Act
        var result = await controller.Get(testedModel);


        //// Assert
        Assert.NotNull(result);
        var actionResult = Assert.IsAssignableFrom<IActionResult>(result);
        var okObjectResult = Assert.IsType<OkObjectResult>(actionResult);
        Assert.NotNull(okObjectResult);

        _output.WriteLine(okObjectResult.Value.Serialize());

        Assert.IsType<Block>(okObjectResult.Value);
    }


    [Theory]
    [ClassData(typeof(IncorrectQueryTestData))]
    public async void Get_ReturnBadRequestIfInvalidQuery(IpAddressModel testedModel)
    {
        //// Arrange
        var controller = new GeoIpController
        (
            _geoIpRepositoryMock.Object,
            _localization
        );
        

        //// Act
        var result = await controller.Get(testedModel);


        //// Assert
        Assert.NotNull(result);
        var actionResult = Assert.IsAssignableFrom<IActionResult>(result);
        var badObjectResult = Assert.IsType<BadRequestObjectResult>(actionResult);
        Assert.NotNull(badObjectResult);

        _output.WriteLine(badObjectResult.Value.Serialize());

        var errorResult = Assert.IsType<ErrorResult>(badObjectResult.Value);

        Assert.NotEmpty(errorResult.Errors);
    }


    [Theory]
    [ClassData(typeof(IncorrectIpAddressTestData))]
    public async void Get_ReturnUnprocessableIfIpInvalid(IpAddressModel testedModel)
    {
        //// Arrange
        var controller = new GeoIpController
        (
            _geoIpRepositoryMock.Object,
            _localization
        );


        //// Act
        var result = await controller.Get(testedModel);


        //// Assert
        Assert.NotNull(result);
        var actionResult = Assert.IsAssignableFrom<IActionResult>(result);
        var unprocessableObjectResult = Assert.IsType<UnprocessableEntityObjectResult>(actionResult);
        Assert.NotNull(unprocessableObjectResult);

        _output.WriteLine(unprocessableObjectResult.Value.Serialize());

        var errorResult = Assert.IsType<ErrorResult>(unprocessableObjectResult.Value);

        Assert.NotEmpty(errorResult.Errors);
    }
    #endregion _Methods.UnitTests


    #region Methods.Helpers
    /// <summary>
    ///     Mock user-services
    /// </summary>
    private void SetupServices()
    {
        _geoIpRepositoryMock
           .Setup(m => m.GetByIpAsync(It.Is<string>(s => StoredNetworks.Contains(s))))
           .ReturnsAsync(new Block());
        
        _geoIpRepositoryMock
           .Setup(m => m.GetByIpAsync(It.Is<string>(s => !StoredNetworks.Contains(s))))
           .ReturnsAsync(new CommonStates.NotFound());
    }
    #endregion _Methods.Helpers
}