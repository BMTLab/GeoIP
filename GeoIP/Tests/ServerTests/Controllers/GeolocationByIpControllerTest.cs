#region HEADER
//   GeolocationByIpControllerTest.cs of GeoIP.Tests
//   Created by Nikita Neverov at 21.01.2020 9:09
#endregion


using System;
using System.Net;

using GeoIP.Server.Controllers;
using GeoIP.Server.Services.DataProviders;
using GeoIP.Shared.Models;
using GeoIP.Tests.ServerTests.Services.DataProviders;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using Newtonsoft.Json;

using Xunit;
using Xunit.Abstractions;


namespace GeoIP.Tests.ServerTests.Controllers
{
    public sealed class GeolocationByIpControllerTest
    {
        #region Fields
        private static readonly IGeoIpProvider FakeTrackerProvider = new FakeGeoIpProvider();
        private readonly ITestOutputHelper _output;
        #endregion
        
        #region Constructors
        static GeolocationByIpControllerTest() => GC.Collect();


        public GeolocationByIpControllerTest(ITestOutputHelper output)
        {
            _output = output;
        }
        #endregion
        
        
        #region Methods.Tests
        
        [Fact]
        public async void GetReturnValidJsonDataWhenIpIsCorrect()
        {
            // Arrange
            const string ip = "1.1.1.1";
            var controller = new GeolocationByIpController(FakeTrackerProvider);

            // Act
            var result = await controller.GetAsync(ip);

            // Assert
            var actionResult = Assert.IsAssignableFrom<IActionResult>(result);
            var jsonResult = Assert.IsType<JsonResult>(actionResult);
            Assert.NotNull(jsonResult);
            
            var obj = (Block) jsonResult.Value;
            Assert.Equal("Chicago", obj.Location.CityName);
            Assert.Equal((IPAddress.Parse(ip), 24), obj.Network);
            
            _output.WriteLine(JsonConvert.SerializeObject(obj));
        }


        [Theory]
        [InlineData("a.b.c.d", "0.0.0.0")]
        public async void GetReturnModelErrorWhenIpIsIncorrect(params string[] ipRequests)
        {
            // Arrange
            var controller = new GeolocationByIpController(FakeTrackerProvider);

            // Act
            foreach (var ip in ipRequests)
            {
                var result = await controller.GetAsync(ip);

                // Assert
                var actionResult = Assert.IsAssignableFrom<IActionResult>(result);
                var badResult = Assert.IsType<BadRequestResult>(actionResult);
                Assert.NotNull(badResult);
                Assert.Equal(400, badResult.StatusCode);
                Assert.Equal(ModelValidationState.Invalid, controller.ModelState.ValidationState);

                _output.WriteLine(JsonConvert.SerializeObject(badResult));
            }
        }
        
        #endregion
    }
}