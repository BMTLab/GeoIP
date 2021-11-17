namespace Shared.Services.SecretsBridge.SecretsModels;

public interface IJwtOptions
{
    string JwtSecurityKey { get; }
    string JwtIssuer { get; }
    string JwtAudience { get; }
    string JwtExpiryInDays { get; }
}