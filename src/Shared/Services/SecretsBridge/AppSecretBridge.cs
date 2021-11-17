using Microsoft.Extensions.Options;

using Shared.Services.Crypto.Decryptor;
using Shared.Services.Crypto.Decryptor.Extensions;
using Shared.Services.SecretsBridge.SecretsModels;
using Shared.Services.SecretsBridge.SecretsModels.Abstractions;


namespace Shared.Services.SecretsBridge;

public class AppSecretBridge : IAppSecretsResolved
{
    #region Ctors
    public AppSecretBridge
    (
        IOptions<AppSecrets> appSecretsOptions,
        ISecretDecrypt decryptor,
        ISecretValidator<IAppSecretsStructure> validator
    )
    {
        if (appSecretsOptions is null)
            throw new ArgumentNullException(nameof(appSecretsOptions));

        _appSecrets = appSecretsOptions.Value ?? throw new ArgumentNullException(nameof(appSecretsOptions));
        _decryptor = decryptor ?? throw new ArgumentException($"Decryptor in {nameof(AppSecretBridge)} is null", nameof(decryptor));

        if (validator is null)
            throw new ArgumentNullException(nameof(validator));

        if (!validator.TryValidate(_appSecrets, out var validationException))
            throw validationException;
    }
    #endregion


    #region Fields
    private readonly AppSecrets _appSecrets;
    private readonly ISecretDecrypt _decryptor;
    #endregion


    #region Implementation of IJwtOptions
    public string JwtSecurityKey => _appSecrets.JwtOptions.Decrypt(_decryptor, false, nameof(JwtSecurityKey));
    public string JwtIssuer => _appSecrets.JwtOptions[nameof(IJwtOptions.JwtAudience)];
    public string JwtAudience => _appSecrets.JwtOptions[nameof(IJwtOptions.JwtIssuer)];
    public string JwtExpiryInDays => _appSecrets.JwtOptions[nameof(IJwtOptions.JwtExpiryInDays)];
    #endregion


    #region Implementation of IConnectionOptions
    public ConnectionOption GeoIpDb =>
        new ConnectionOption
        {
            ConnectionString = _appSecrets.ConnectionOptions[nameof(GeoIpDb)].ConnectionString.DecryptValue(_decryptor),
            DatabaseName = _appSecrets.ConnectionOptions[nameof(GeoIpDb)].DatabaseName.DecryptValue(_decryptor)
        };
    #endregion
}