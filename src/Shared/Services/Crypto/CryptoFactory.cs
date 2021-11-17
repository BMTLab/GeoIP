using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;


namespace Shared.Services.Crypto;

public class CryptoFactory : ICryptoFactory
{
    #region FactoryCtors
    public virtual ICryptoAlgorithm CreateAes(string password, string salt) =>
        new CryptoAlgorithm(password, salt, Aes.Create());
    #endregion


    private sealed class CryptoAlgorithm : ICryptoAlgorithm
    {
        #region Fileds
        private readonly ICryptoTransform _encryptor;
        private readonly ICryptoTransform _decryptor;
        #endregion


        #region Methods.Implements
        [SuppressMessage("Symmetric encryption uses a different initialization vector from the default, which can lead to reproducibility", "CA5401")]
        public CryptoAlgorithm(string password, string salt, SymmetricAlgorithm algorithm)
        {
            if (password is null)
                throw new ArgumentNullException(nameof(password));

            if (salt is null)
                throw new ArgumentNullException(nameof(salt));

            DeriveBytes rgb = new Rfc2898DeriveBytes(password, Encoding.Unicode.GetBytes(salt));
                
            var rgbKey = rgb.GetBytes(algorithm.KeySize >> 3);
            var rgbIv = rgb.GetBytes(algorithm.BlockSize >> 3);
            _encryptor = algorithm.CreateEncryptor(rgbKey, rgbIv);
            _decryptor = algorithm.CreateDecryptor(rgbKey, rgbIv);

            rgb.Dispose();
            algorithm.Dispose();
        }


        public string Encrypt(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            using var buffer = new MemoryStream();

            using (var stream = new CryptoStream(buffer, _encryptor, CryptoStreamMode.Write))
            {
                using var writer = new StreamWriter(stream, Encoding.Unicode);

                writer.Write(text);
            }

            return Convert.ToBase64String(buffer.ToArray());
        }


        public async Task<string> EncryptAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            await using var buffer = new MemoryStream();

            await using (var stream = new CryptoStream(buffer, _encryptor, CryptoStreamMode.Write))
            {
                await using var writer = new StreamWriter(stream, Encoding.Unicode);

                await writer.WriteAsync(text);
            }

            return Convert.ToBase64String(buffer.ToArray());
        }


        public string Decrypt(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            using var buffer = new MemoryStream(Convert.FromBase64String(text));

            using var stream = new CryptoStream(buffer, _decryptor, CryptoStreamMode.Read);

            using var reader = new StreamReader(stream, Encoding.Unicode);

            return reader.ReadToEnd();
        }


        public async Task<string> DecryptAsync(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            await using var buffer = new MemoryStream(Convert.FromBase64String(text));

            await using var stream = new CryptoStream(buffer, _decryptor, CryptoStreamMode.Read);

            using var reader = new StreamReader(stream, Encoding.Unicode);

            return await reader.ReadToEndAsync();
        }


        public string HashKey(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            var plainTextBytes = Encoding.UTF8.GetBytes(text);
                
            using var hash = SHA384.Create();

            hash.Initialize();

            return Convert.ToBase64String(hash.ComputeHash(plainTextBytes));
        }
        #endregion
    }
}