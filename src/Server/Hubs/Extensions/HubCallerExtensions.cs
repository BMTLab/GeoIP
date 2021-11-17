using Microsoft.AspNetCore.SignalR;

using Server.Hubs.Constants;


namespace Server.Hubs.Extensions;


public static class HubCallerExtensions
{
    public static T Roots<T>(this IHubClients<T> clients) =>
        clients.Group(GroupNames.Roots);
}