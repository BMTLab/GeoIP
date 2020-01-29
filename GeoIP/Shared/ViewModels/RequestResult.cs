#region HEADER
//   RequestResult.cs of GeoIP.Shared
//   Created by Nikita Neverov at 29.01.2020 9:18
#endregion


using GeoIP.Shared.Models;

using JetBrains.Annotations;


namespace GeoIP.Shared.ViewModels
{
    public sealed class RequestResult
    {
        public bool Successful { get; set; }
        
        public string? Error { get; set; }
        
        [UsedImplicitly]
        public Block? GeoLocation { get; set; }
    }
}