namespace Shared.Services.Crypto.Decryptor;

public interface ISecretDecrypt
{
    string Decrypt(string key, IDictionary<string, string> keyValues, bool isHashed = true);
    string DecryptValue(string encodedString);
}