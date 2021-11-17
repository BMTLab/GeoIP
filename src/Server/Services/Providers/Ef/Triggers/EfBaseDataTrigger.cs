using System.ComponentModel;
using System.Text;

using EntityFrameworkCore.Triggered;

using JetBrains.Annotations;

using Microsoft.AspNetCore.SignalR;

using Server.Hubs;

using Shared.Providers.Models;
using Shared.Utils;
using Shared.Utils.TypeExtensions;
using Shared.Utils.TypeExtensions.JsonSerializerExtensions;


namespace Server.Services.Providers.Ef.Triggers;

[UsedImplicitly]
public abstract class EfBaseDataTrigger<TEntity> : IAfterSaveTrigger<TEntity> where TEntity : class 
{
    #region Fields
    protected readonly ILogger<EfBaseDataTrigger<TEntity>>? Logger;
    protected readonly IHubContext<ClientHub> HubContext;
    #endregion


    #region Ctors
    protected EfBaseDataTrigger
    (
        IHubContext<ClientHub> hubContext,
        ILogger<EfBaseDataTrigger<TEntity>>? logger = null
    )
    {
        HubContext = hubContext;
        Logger = logger;
    }
    #endregion


    #region Properties
    [DefaultValue(true)]
    protected bool LogEnable { get; set; } = true;

    [DefaultValue("")]
    protected string HubMethodName { get; set; } = string.Empty;
    #endregion _Properties



    #region Implementation of IAfterSaveTrigger<TEntity>
    public async Task AfterSave(ITriggerContext<TEntity> context, CancellationToken cancellationToken)
    {
        var entity = context.Entity;
        var old = context.UnmodifiedEntity;
        var changeType = context.ChangeType switch
        {
            ChangeType.Added    => DataChangeType.Added,
            ChangeType.Deleted  => DataChangeType.Deleted,
            ChangeType.Modified => DataChangeType.Modified,
            var _               => throw new ArgumentOutOfRangeException(nameof(context), context.ChangeType, @$"Unknown {nameof(ChangeType)}")
        };


        HashSet<string>? changedProperties = null;
        if (old != null && changeType == DataChangeType.Modified)
            changedProperties = new HashSet<string>(ReplaceUtil<TEntity>.DetectChanged(old, entity).Select(p => p.Name));

        if (!cancellationToken.IsCancellationRequested)
            await HandleTriggerAsync(entity, changeType, changedProperties, cancellationToken);
        
        if (LogEnable)
        {
            var sb = new StringBuilder();
            sb.Append($"Change in {typeof(TEntity).Name}. Type is {Enum.GetName(changeType)}");
            sb.Append($".\n New entity: {entity.Serialize()}");
            
            if (old != null)
                sb.Append($".\n Old entity: {old.Serialize()}");

            if (changedProperties.IsValid())
                sb.Append($".\n Changed: {string.Join(", ", changedProperties)}");
            
            Logger?.LogInformation(sb.ToString());
        }
    }
    #endregion


    protected abstract Task HandleTriggerAsync
    (
        TEntity entity,
        DataChangeType changeType,
        HashSet<string>? changedProperties = null,
        CancellationToken cancellationToken = default
    );
}