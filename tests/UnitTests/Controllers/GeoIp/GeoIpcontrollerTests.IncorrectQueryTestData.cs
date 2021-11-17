using System.Collections;

using Shared.ViewModels;


namespace GeoIp.Tests.Controllers.GeoIp;

public sealed partial class GeoIpControllerTests
{
    #region DataGenerators
    private sealed class IncorrectQueryTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new IpAddressModel { Ip = null } };
            yield return new object[] { new IpAddressModel { Ip = string.Empty }};
        }


        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
    #endregion _DataGenerators
}