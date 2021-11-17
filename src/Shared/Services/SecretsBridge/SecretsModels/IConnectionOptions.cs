using Shared.Services.SecretsBridge.SecretsModels.Abstractions;


namespace Shared.Services.SecretsBridge.SecretsModels;

public interface IConnectionOptions
{
    ConnectionOption GeoIpDb { get; }
}