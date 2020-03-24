using System;
using System.Security.Cryptography;
using System.Text;

namespace SecurityExamples.Hmac
{
    public class HmacSimpleExample
    {
        //     HMAC combine a one-way hash function with a secret cryptographic key you get a hash
        //     message authentication code (HMAC)

        public static void Execute()
        {
            const int keySize = 32;
            var key = GenerateKey(keySize);

            const string msg = "Hello, world!";
            var msgBytes = Encoding.UTF8.GetBytes(msg);

            var hashBytes = ComputeHash(msgBytes, key);

            Console.WriteLine(Convert.ToBase64String(hashBytes));
        }

        private static byte[] ComputeHash(byte[] msg, byte[] key)
        {
            var hmac = new HMACSHA512(key);

            return hmac.ComputeHash(msg);
        }

        private static byte[] GenerateKey(int length)
        {
            var randomNumber = new byte[length];

            using var rng = new RNGCryptoServiceProvider();

            rng.GetBytes(randomNumber);

            return randomNumber;
        }
    }
}
