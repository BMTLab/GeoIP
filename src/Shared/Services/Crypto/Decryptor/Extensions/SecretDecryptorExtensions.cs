using System.Runtime.CompilerServices;


namespace Shared.Services.Crypto.Decryptor.Extensions;

public static class DecryptExtensions
{
    /// <remarks>Extension method.</remarks>
    /// <summary>
    ///     Decrypts value by hashed key in the dictionary encodedKeyValue />,
    ///     by default the key name is the name of the called property using <c>[CallerMemberName]</c>
    /// </summary>
    /// <param name="encodedKeyValue"></param>
    /// <param name="decryptor"></param>
    /// <param name="isHashed"></param>
    /// <param name="propName"></param>
    /// <returns>string</returns>
    public static string Decrypt
    (
        this IDictionary<string, string> encodedKeyValue,
        ISecretDecrypt decryptor,
        bool isHashed = true,
        [CallerMemberName] string propName = ""
    )
    {
        if (decryptor is null)
            throw new ArgumentNullException(nameof(decryptor));

        return decryptor.Decrypt(propName, encodedKeyValue, isHashed);
    }


    public static string DecryptValue
    (
        this string encodedString,
        ISecretDecrypt decryptor
    )
    {
        if (decryptor is null)
            throw new ArgumentNullException(nameof(decryptor));

        return decryptor.DecryptValue(encodedString);
    }
}