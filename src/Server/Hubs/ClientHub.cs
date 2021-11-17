using JetBrains.Annotations;

using Localization;

using Microsoft.AspNetCore.Authorization;

using Server.Hubs.Abstractions;


namespace Server.Hubs;

[Authorize]
[PublicAPI]
public class ClientHub : BaseHub<ClientHub>
{
    #region Fields
    #endregion _Fields


    #region Ctors
    public ClientHub
    (
        ILocalization lc, 
        ILogger<ClientHub>? logger = null
    ) : base(lc, logger)
    {
    }
    #endregion


    #region Methods
    #endregion _Methods
}