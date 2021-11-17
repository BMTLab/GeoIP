using JetBrains.Annotations;

using NullGuard;


namespace Shared.Services.SecretsBridge.SecretsModels.Abstractions;

[UsedImplicitly]
[NullGuard(ValidationFlags.None)]
public class ConnectionOption
{
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
}