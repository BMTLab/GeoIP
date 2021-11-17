

namespace Shared.Services.Crypto;

public interface ICryptoFactory
{
    ICryptoAlgorithm CreateAes(string password, string salt);
}