using NullGuard;

using Shared.Services.SecretsBridge.SecretsModels.Abstractions;


namespace Shared.Services.SecretsBridge;

/// <summary>
///     Class reflection of all fields in the configuration section of AppSecrets
/// </summary>
[NullGuard(ValidationFlags.None)]
public class AppSecrets : IAppSecretsStructure
{
    public Dictionary<string, string> JwtOptions { get; } = new(4);

    public Dictionary<string, ConnectionOption> ConnectionOptions { get; } = new(2);
}