using System.Security.Cryptography;
using System.Text;
using System;
using System.IO;

namespace passwordManagent2.services
{
    public class EncryptionPassword
    {
        public EncryptionPassword()
        {

        }
        public string Encrypt(string plainPassword, string key)
        {
            // convertimos el texto y la clave en un arreglo de bytes
            byte[] plainBytes = Encoding.UTF8.GetBytes(plainPassword);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            // ajusta la clave para que sea de 256 bits
            using (var aes = Aes.Create())
            {
                aes.Key = SHA256.Create().ComputeHash(keyBytes);
                aes.IV = new byte[16];

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                {
                    cs.Write(plainBytes, 0, plainBytes.Length);
                    cs.Close();
                    return Convert.ToBase64String(ms.ToArray());

                }
            }
        }

        public string Decrypt(string encryptedPassword, string key)
        {
            byte[] encrytedBytes = Convert.FromBase64String(encryptedPassword);
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            using (var aes = Aes.Create())
            {
                using (var decryptor = aes.CreateDecryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream(encrytedBytes))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var reader = new StreamReader(cs))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
