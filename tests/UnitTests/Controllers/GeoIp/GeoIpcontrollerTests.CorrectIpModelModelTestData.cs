using System.Collections;

using Shared.ViewModels;


namespace GeoIp.Tests.Controllers.GeoIp;

public partial class GeoIpControllerTests
{
    #region DataGenerators
    private sealed class CorrectIpModelModelTestData : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[]
            {
                new IpAddressModel
                {
                    Ip = StoredNetworks[0]
                }
            };
        }


        IEnumerator IEnumerable.GetEnumerator() =>
            GetEnumerator();
    }
    #endregion _DataGenerators
}