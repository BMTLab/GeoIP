using NullGuard;

using Shared.Utils.TypeExtensions;


namespace Shared.Services.Crypto.Decryptor;

[NullGuard(ValidationFlags.None)]
public class SecretValidator<T> : ISecretValidator<T> where T : class
{
    public bool TryValidate(T settings, out AggregateException validationExceptions)
    {
        if (settings is null)
            throw new ArgumentNullException(nameof(settings));

        var exceptions = new List<Exception>();

        ValidateDictionaries(settings, ref exceptions);
        ValidateSingle(settings, ref exceptions);
        validationExceptions = new AggregateException(exceptions);

        return !exceptions.Any();
    }


    private static void ValidateDictionaries(T settings, ref List<Exception> exceptions)
    {
        var collections = settings.GetTypedProperties<T, Dictionary<string, string>>(2);

        foreach (var (collection, name) in collections)
            if (!collection.IsValid())
                exceptions.Add(new ArgumentNullException(name));
            else
                foreach (var (key, value) in collection)
                {
                    if (!key.IsValid())
                        exceptions.Add(new ArgumentNullException(name, nameof(key)));

                    if (!value.IsValid())
                        exceptions.Add(new ArgumentNullException(name, nameof(value)));
                }
    }


    private static void ValidateSingle(T settings, ref List<Exception> exceptions)
    {
        var strings = settings.GetTypedProperties<T, string>(2);

        foreach (var (value, name) in strings)
            if (!value.IsValid())
                exceptions.Add(new ArgumentNullException(name, nameof(value)));
    }
}