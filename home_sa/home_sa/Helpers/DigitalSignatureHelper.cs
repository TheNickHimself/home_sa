using System.Security.Cryptography;
using System.Text;

namespace home_sa.Helpers
{
    public static class DigitalSignatureHelper
    {
        public static byte[] SignData(byte[] data, string privateKey)
        {

            using(var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.ImportRSAPrivateKey(Convert.FromBase64String(privateKey), out _);
                return rsa.SignData(data, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
            }
        }

        public static bool VerifyData(byte[] data, byte[] signature, string publicKey)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                rsa.ImportRSAPublicKey(Convert.FromBase64String(publicKey), out _);
                return rsa.VerifyData(data, signature, HashAlgorithmName.SHA512, RSASignaturePadding.Pkcs1);
            }
        }
    }

}
