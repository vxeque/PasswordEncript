using System.Security.Cryptography;
using System.Text;
using System;
using System.IO;

namespace passwordManagent2.services
{
    public class EncryptionPassword : IDisposable
    {
        private bool disposed = false;

        public string Encrypt(string plainPassword, string key)
        {
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainPassword);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            using (var aes = Aes.Create())
            {
                aes.Key = SHA256.HashData(keyBytes); // Uso de HashData más moderno
                aes.IV = new byte[16]; // IV de ceros para consistencia

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(plainBytes, 0, plainBytes.Length);
                    cs.FlushFinalBlock(); // Importante: asegura que todos los datos se escriban
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        public string Decrypt(string encryptedPassword, string key)
        {
            byte[] encryptedBytes = Convert.FromBase64String(encryptedPassword);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            using (var aes = Aes.Create())
            {
                // ¡AQUÍ ESTABA EL ERROR! Faltaba configurar Key e IV
                aes.Key = SHA256.HashData(keyBytes);
                aes.IV = new byte[16]; // Mismo IV que en encrypt

                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream(encryptedBytes))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var reader = new StreamReader(cs))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed && disposing)
            {
                // Cleanup si es necesario
            }
            disposed = true;
        }
    }
}
