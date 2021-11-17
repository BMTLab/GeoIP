namespace Shared.Services.Crypto.Decryptor;

public class SecretDecryptorFactory
{
    public virtual ISecretDecrypt Create(ICryptoAlgorithm crypto) =>
        new SecretDecryptor(crypto);


    private sealed class SecretDecryptor : ISecretDecrypt
    {
        #region Fileds
        private readonly ICryptoAlgorithm _crypto;
        #endregion


        #region Ctors
        public SecretDecryptor(ICryptoAlgorithm crypto)
        {
            _crypto = crypto ?? throw new ArgumentNullException(nameof(crypto));
        }
        #endregion


        #region Methods
        public string Decrypt(string key, IDictionary<string, string> keyValues, bool isHashed = true)
        {
            var hashedKey = isHashed ? _crypto.HashKey(key) : key;
            var value = keyValues[hashedKey];

            return DecryptValue(value);
        }


        public string DecryptValue(string encodedString) =>
            _crypto.Decrypt(encodedString);
        #endregion
    }
}