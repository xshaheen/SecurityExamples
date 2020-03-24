using System.Security.Cryptography;

namespace SecurityExamples.Asymmetric.Rsa
{
    public class InMemoryKeyExample
    {
        private RSAParameters _publicKey;
        private RSAParameters _privateKey;

        public void AssignNewKey()
        {
            using var rsa = new RSACryptoServiceProvider(4096) { PersistKeyInCsp = false };

            _publicKey = rsa.ExportParameters(false);
            _privateKey = rsa.ExportParameters(true);
        }
    }
}
