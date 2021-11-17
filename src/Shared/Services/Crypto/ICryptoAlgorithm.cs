namespace Shared.Services.Crypto;

public interface ICryptoAlgorithm
{
    string Encrypt(string text);
    Task<string> EncryptAsync(string text);
    string Decrypt(string text);
    Task<string> DecryptAsync(string text);
    string HashKey(string text);
}