using Microsoft.AspNetCore.SignalR.Client;


namespace Shared.Providers.Models;

public class HubStateChangedEventArgs : EventArgs
{
    public static readonly HubStateChangedEventArgs Connected = 
        new Lazy<HubStateChangedEventArgs>(() => new HubStateChangedEventArgs(HubConnectionState.Connected), LazyThreadSafetyMode.PublicationOnly).Value;
        
    public static readonly HubStateChangedEventArgs Connecting = 
        new Lazy<HubStateChangedEventArgs>(() => new HubStateChangedEventArgs(HubConnectionState.Connecting), LazyThreadSafetyMode.PublicationOnly).Value;
        
    public static readonly HubStateChangedEventArgs Disconnected = 
        new Lazy<HubStateChangedEventArgs>(() => new HubStateChangedEventArgs(HubConnectionState.Disconnected), LazyThreadSafetyMode.PublicationOnly).Value;
        
    public static readonly HubStateChangedEventArgs Reconnecting = 
        new Lazy<HubStateChangedEventArgs>(() => new HubStateChangedEventArgs(HubConnectionState.Reconnecting), LazyThreadSafetyMode.PublicationOnly).Value;
        
        
        
    public HubStateChangedEventArgs(HubConnectionState newState) =>
        NewState = newState;

    public HubConnectionState NewState { get; }
}