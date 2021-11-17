using System.Diagnostics.CodeAnalysis;

using FluentValidation;

using Localization;

using Microsoft.AspNetCore.SignalR;

using Server.Hubs.Constants;

using Shared.ViewModels.Results.Abstractions;
using Shared.ViewModels.Validators.Abstractions;

using static Shared.ViewModels.Results.Extensions.ResultExtensions;


namespace Server.Hubs.Abstractions;


[SuppressMessage("ReSharper", "ContextualLoggerProblem")]
public abstract class BaseHub<T> : Hub
{
    #region Fields
    protected readonly ILocalization Lc;
    protected readonly ILogger<T>? Logger;
    #endregion _Fields

        
    #region Ctors
    protected BaseHub
    (
        ILocalization lc, 
        ILogger<T>? logger = null
    )
    {
        Logger = logger;
        Lc = lc;
    }
    #endregion
        

    #region Methods
    #region Overrides of Hub
    public override async Task OnConnectedAsync()
    {
        var connectionId = Context.ConnectionId;
        var user = Context.User!;
        var ip = Context.GetHttpContext()?.Connection.RemoteIpAddress;

        var userName = user.Identity?.Name ?? "Anonymous";

        Logger?.LogInformation($"User connected {userName} {ip}");
        
        await Groups.AddToGroupAsync(connectionId, GroupNames.Users);
    }


    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var connectionId = Context.ConnectionId;
        var userName = Context.User?.Identity?.Name ?? "Anonymous";
            
        if (exception != null)
            Logger?.LogError(exception, $"User {userName} with connectionId {connectionId} disconnected with error {exception.Message}");
            
        await Task.WhenAll(
            Groups.RemoveFromGroupAsync(connectionId, GroupNames.Roots),
            Groups.RemoveFromGroupAsync(connectionId, GroupNames.Users));
    }
    #endregion _Overrides of Hub
        
        
    protected private bool IsValidationFailed(ILocalizedValidatableObject model, [NotNullWhen(true)] out IBaseResult? returnAction) =>
        !TryValidateModel(model, out returnAction);

        
    protected private bool TryValidateModel(ILocalizedValidatableObject model, [NotNullWhen(false)] out IBaseResult? returnAction)
    {
        if (model is null)
            throw new ArgumentNullException(nameof(model));

        if (!model.IsValid(Lc, out var errors))
        {
            returnAction = 
                Error(errors.Any(e => e.Severity == Severity.Error) 
                          ? errors.Where(e => e.Severity == Severity.Error).Select(p => p.ErrorMessage) 
                          : errors.Where(e => e.Severity != Severity.Error).Select(p => p.ErrorMessage));

            return false;
        }

        returnAction = null;

        return true;
    }
    #endregion _Methods
}