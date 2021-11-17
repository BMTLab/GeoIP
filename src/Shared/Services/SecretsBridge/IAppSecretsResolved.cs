using Shared.Services.SecretsBridge.SecretsModels;


namespace Shared.Services.SecretsBridge;

public interface IAppSecretsResolved
    : IJwtOptions, IConnectionOptions
{
}