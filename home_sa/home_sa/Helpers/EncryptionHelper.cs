using System.IO;
using System.Security.Cryptography;
using System.Text;
using System;
using System.Security.Cryptography.X509Certificates;

namespace home_sa.Helpers
{


    public static class EncryptionHelper
    {
        public static byte[] EncryptFile(byte[] fileData, RSA rsa, out byte[] encryptedSymmetricKey, out byte[] iv)
        {
            Aes aes = Aes.Create();
            aes.KeySize = 256;
            aes.GenerateKey();
            aes.GenerateIV();

            byte[] aesKey = aes.Key;
            byte[] aesIV = aes.IV;

            encryptedSymmetricKey = rsa.Encrypt(aesKey, RSAEncryptionPadding.Pkcs1);


            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = aesKey;
                aesAlg.IV = aesIV;
                iv = aesAlg.IV;


                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        csEncrypt.Write(fileData, 0, fileData.Length);
                        //msEncrypt.Position = 0;//maybe not needed
                        csEncrypt.Close();
                        msEncrypt.Flush();

                    }
                    return msEncrypt.ToArray();
                }
            }
        }

        public static byte[] DecryptFile(byte[] encryptedFileData, byte[] symmetricKey, byte[] iv)
        {

            using (var aes = Aes.Create())
            {
                //aes.Key
                //var symmetricKey = rsa.Decrypt(encryptedSymmetricKey, RSAEncryptionPadding.Pkcs1);

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
