using System.Collections;

using Shared.ViewModels;


namespace GeoIp.Tests.Controllers.GeoIp;

public partial class GeoIpControllerTests
{
    #region DataGenerators
    private sealed class IncorrectIpAddressTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new IpAddressModel { Ip = "33.FF.33.33" } };
            yield return new object[] { new IpAddressModel { Ip = "678.10.10.10" } };
            yield return new object[] { new IpAddressModel { Ip = "33.33.33.33 " } };
            yield return new object[] { new IpAddressModel { Ip = "33.33.33.33.33" } };
        }


        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
    #endregion _DataGenerators
}