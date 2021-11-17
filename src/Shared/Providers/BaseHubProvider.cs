using System.Runtime.CompilerServices;

using JetBrains.Annotations;

using Localization;

using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using OneOf;
using OneOf.Types;

using Shared.Providers.Abstractions;
using Shared.Providers.Models;
using Shared.Utils.TypeExtensions.JsonSerializerExtensions;
using Shared.ViewModels.Results;
using Shared.ViewModels.Results.Abstractions;


namespace Shared.Providers;

public abstract class BaseHubProvider : IBaseProvider<HubConnection, IBaseResult>, IAsyncDisposable
{
    #region Fields
    protected readonly ILocalization L;
    protected readonly ILogger<BaseHubProvider>? Logger;
    #endregion

        
    #region Ctors
    protected BaseHubProvider
    (
        Uri hubPath,
        Func<Task<string>> tokenProvider,
        ILoggerProvider hubLoggerProvider,
        ILocalization localization,
        IRetryPolicy retryPolicy,
        ILogger<BaseHubProvider>? logger = null
    )
    {
        L = localization;
        Logger = logger;
            
        Connection =
            new HubConnectionBuilder()
               .WithUrl
                (
                    hubPath,
                    options =>
                    {
                        options.AccessTokenProvider = tokenProvider!;
                        options.Transports = HttpTransportType.WebSockets;
                        options.SkipNegotiation = true;
                    }
                )
               .AddJsonProtocol(options => options.PayloadSerializerOptions.SetOptions())
               .ConfigureLogging(f => f.AddProvider(hubLoggerProvider))
                //.AddMessagePackProtocol(options => WsMessagePackSerializerOptions.SetOptions(options.SerializerOptions))
               .WithAutomaticReconnect(retryPolicy)
               .Build();

        ConfigureEvents();

        Logger?.LogInformation("Created");
    }
    #endregion
        
        
    #region Properties
    [PublicAPI]
    public HubConnectionState State => Connection.State;


    [PublicAPI]
    public HubConnection Connection { get; protected set; }
    #endregion _Properties


    #region Methods
    [PublicAPI]
    public virtual async Task ConnectAsync()
    {
        if (Connection is { State: HubConnectionState.Disconnected })
        {
            await Connection.StartAsync();
            await ConnectionOnChangeAsync();
        }
    }


    [PublicAPI]
    public virtual async Task DisconnectAsync()
    {
        if (Connection is { State: HubConnectionState.Connected })
            await Connection.StopAsync();
    }



    #region Overrides of BaseProvider
    public async Task<OneOf<TSuccess, ErrorResult>> HandleRequestAsync<TSuccess>
    (
        Func<HubConnection, Task<IBaseResult>> request,
        [CallerMemberName] string memberName = ""
    ) where TSuccess : notnull, new()
    {
        if (request is null)
            throw new ArgumentNullException(nameof(request));

        if (Connection is not { State: HubConnectionState.Connected })
            return new ErrorResult(L.ErrorLostConnection);

        try
        {
            var response = await request.Invoke(Connection);

            return await HandleResponseAsync<TSuccess>(response, memberName);
        }
        catch (Exception exc)
        {
            Logger?.LogError(exc, $"An unexpected error occurred while processing the request {memberName}");

            return new ErrorResult(L.ErrorInternal);
        }
    }
        

    protected async Task<OneOf<TSuccess, ErrorResult>> HandleResponseAsync<TSuccess>
    (
        IBaseResult response, 
        string memberName
    ) where TSuccess: notnull, new()
    {
        if (response is null)
            throw new ArgumentNullException(nameof(response));

        if (response.IsSuccessful)
        {
            if (typeof(TSuccess) == typeof(Success))
                return await Task.FromResult(new TSuccess());

            return ((ContentResult<TSuccess>) response).Success;
        }

        var errorResult = (ErrorResult) response;

        Logger?.LogWarning($"{memberName} return errors: {string.Join(", ", errorResult.Errors)}");

        return errorResult;
    }
    #endregion _Overrides of BaseProvider
        
        
    private void ConfigureEvents()
    {
        Connection.Closed += ConnectionOnChangeAsync;
            
        Connection.Reconnecting += ConnectionOnChangeAsync;
            
        Connection.Reconnected += ConnectionOnChangeAsync;
    }


    private async Task ConnectionOnChangeAsync(object? _ = null)
    {
        await Task.Yield();

        var newStateArgs = Connection.State switch
        {
            HubConnectionState.Connected    => HubStateChangedEventArgs.Connected,
            HubConnectionState.Connecting   => HubStateChangedEventArgs.Connecting,
            HubConnectionState.Reconnecting => HubStateChangedEventArgs.Reconnecting,
            var _                           => HubStateChangedEventArgs.Disconnected
        };
            
        StateChanged(this, newStateArgs);
    }
    #endregion _Methods


    #region Events
    public event EventHandler<HubStateChangedEventArgs> StateChanged = delegate { };
    #endregion _Events

        
    #region Implementation of IAsyncDisposable
    public async ValueTask DisposeAsync()
    {
        GC.SuppressFinalize(this);

        await DisconnectAsync();
            
        if (Connection != null!)
        {
            Connection.Closed -= ConnectionOnChangeAsync;
            Connection.Reconnecting -= ConnectionOnChangeAsync;
            Connection.Reconnected -= ConnectionOnChangeAsync;
            await Connection.DisposeAsync();
        }
            
        Logger?.LogInformation("Disposed");
    }
    #endregion
}