using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;

namespace home_sa.Helpers
{


    public static class EncryptionHelper
    {
        public static byte[] EncryptFile(byte[] fileData, RSA rsa, out byte[] encryptedSymmetricKey, out byte[] iv)
        {
            using (var aes = Aes.Create())
            {
                aes.GenerateKey();
                aes.GenerateIV();
                iv = aes.IV;
                encryptedSymmetricKey = rsa.Encrypt(aes.Key, RSAEncryptionPadding.Pkcs1);

                using (var encryptor = aes.CreateEncryptor(aes.Key, aes.IV))
                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        cs.Write(fileData, 0, fileData.Length);
                        ms.Position = 0;//maybe not needed
                        cs.Close();
                    }
                    return ms.ToArray();
                }
            }
        }

        public static byte[] DecryptFile(byte[] encryptedFileData, RSA rsa, byte[] encryptedSymmetricKey, byte[] iv)
        {
            var symmetricKey = rsa.Decrypt(encryptedSymmetricKey, RSAEncryptionPadding.Pkcs1);

            using (var aes = Aes.Create())
            {
                using (var decryptor = aes.CreateDecryptor(symmetricKey, iv))
                using (var ms = new MemoryStream(encryptedFileData))
                using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                using (var reader = new MemoryStream())
                {
                    cs.CopyTo(reader);
                    return reader.ToArray();
                }
            }
        }
    }

}
