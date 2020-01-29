#region HEADER
//   GeolocationByIpControllerTest.cs of GeoIP.Tests
//   Created by Nikita Neverov at 21.01.2020 9:09
#endregion


using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;

using Fody;

using GeoIP.Server.Controllers;
using GeoIP.Server.Services.DataProviders;
using GeoIP.Shared.Models;
using GeoIP.Tests.ServerTests.Services.DataProviders;

using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;

using Xunit;
using Xunit.Abstractions;


namespace GeoIP.Tests.ServerTests.Controllers
{
    [ConfigureAwait(false)]
    public sealed class GeolocationByIpControllerTest
    {
        #region Fields
        private static readonly IGeoIpProvider FakeProvider = new FakeGeoIpProvider();
        private readonly ITestOutputHelper _output;
        #endregion
        
        
        #region Constructors
        static GeolocationByIpControllerTest() => 
            GC.Collect();
        
        public GeolocationByIpControllerTest(ITestOutputHelper output) => 
            _output = output;
        #endregion
        
        
        #region Methods.Tests

        [Fact]
        public async void GetAsync_ReturnValidJsonDataWhenIpIsCorrect()
        {
            // Arrange
            const string ip = "1.1.1.1";
            var controller = new GeolocationByIpController(FakeProvider);

            // Act
            var result = await controller.GetAsync(ip);

            // Assert
            var actionResult = Assert.IsAssignableFrom<IActionResult>(result);
            var jsonResult = Assert.IsType<OkObjectResult>(actionResult);
            Assert.NotNull(jsonResult);

            var obj = Assert.IsType<Block>(jsonResult.Value);
            Assert.Equal(FakeGeoIpProvider.TestBlock!.Location!.CityName, obj!.Location!.CityName);
            Assert.Equal((IPAddress.Parse(ip), 24), obj.Network);

            _output.WriteLine(JsonConvert.SerializeObject(obj));
        }
        
        
        [Theory]
        [ClassData(typeof(IncorrectIpTestData))]
        public async void GetAsync_ReturnModelErrorWhenIpIsIncorrect(string ipTest)
        {
            // Arrange
            var controller = new GeolocationByIpController(FakeProvider);

            // Act
            var result = await controller.GetAsync(ipTest);

            // Assert
            var actionResult = Assert.IsAssignableFrom<IActionResult>(result);
            var badResult = Assert.IsType<BadRequestObjectResult>(actionResult);
            Assert.NotNull(badResult);
            Assert.Equal(400, badResult.StatusCode);

            _output.WriteLine(JsonConvert.SerializeObject(badResult));
        }
        #endregion _Methods.Tests
        
        
        #region Methods.DataGenerators
        private sealed class IncorrectIpTestData : IEnumerable<object[]>
        {
            public IEnumerator<object[]> GetEnumerator()
            {
                yield return new object[] { null! };
                yield return new object[] { "" };
                yield return new object[] { "  " };
                yield return new object[] { "..." };
                yield return new object[] { " . . . " };
                yield return new object[] { "20.20.20." };
                yield return new object[] { ".20.20.10" };
                yield return new object[] { "a.b.c.d" };
                yield return new object[] { "1.2.3.d" };
                yield return new object[] { "2967453" };
                yield return new object[] { "2.4.6" };
                yield return new object[] { "256.1.2.4" };
                yield return new object[] { "1.1.2.256" };
                yield return new object[] { "1.2.4.6:80" };
            }


            IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        }
        #endregion
    }
}