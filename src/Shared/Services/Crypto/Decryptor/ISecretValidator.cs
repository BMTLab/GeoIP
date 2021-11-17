namespace Shared.Services.Crypto.Decryptor;

public interface ISecretValidator<in T> where T : class
{
    bool TryValidate(T settings, out AggregateException validationExceptions);
}