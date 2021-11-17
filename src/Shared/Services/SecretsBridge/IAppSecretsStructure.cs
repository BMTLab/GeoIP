using Shared.Services.SecretsBridge.SecretsModels.Abstractions;


namespace Shared.Services.SecretsBridge;

/// <summary>
///     The interface of all structure fields of the AppSecrets section
/// </summary>
public interface IAppSecretsStructure
{
    Dictionary<string, string> JwtOptions { get; }
    Dictionary<string, ConnectionOption> ConnectionOptions { get; }
}