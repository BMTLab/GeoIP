#region HEADER
//   IGeoIpProvider.cs of GeoIP.Server
//   Created by Nikita Neverov at 20.01.2020 10:02
#endregion


using System.Threading.Tasks;

using GeoIP.Shared.Models;


namespace GeoIP.Server.Services.DataProviders
{
    public interface IGeoIpProvider
    {
        #region Methods
        Block? GetAllInfoByIp(string ip);
        Task<Block?> GetAllInfoByIpAsync(string ip);
        #endregion
    }
}